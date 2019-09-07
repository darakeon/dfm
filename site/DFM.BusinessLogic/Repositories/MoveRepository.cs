using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keon.Util.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Language;
using DFM.Language.Emails;
using DFM.Language.Extensions;
using Keon.NHibernate.Queries;

namespace DFM.BusinessLogic.Repositories
{
	internal class MoveRepository : GenericMoveRepository<Move>
	{
		internal Move SaveOrUpdate(Move move, DateTime now)
		{
			//Keep this order, weird errors happen if invert
			return SaveOrUpdate(
				move,
				(m) => Validate(
					m,
					now,
					MaxLen.Move_Description,
					DfMError.TooLargeMoveDescription
				),
				Complete
			);
		}

		#region PlaceAccountsInMove
		internal void PlaceMonthsInMove(Move move, Month monthOut, Month monthIn)
		{
			if (monthOut == null)
				move.Out = null;
			else if (!monthOut.OutContains(move))
				monthOut.AddOut(move);
			else
				monthOut.UpdateOut(move);

			if (monthIn == null)
				move.In = null;
			else if (!monthIn.InContains(move))
				monthIn.AddIn(move);
			else
				monthIn.UpdateIn(move);
		}

		#endregion

		#region SendEmail
		internal EmailStatus SendEmail(Move move, OperationType operationType)
		{
			var user = GetUser(move);
			var config = user.Config;

			if (!config.SendMoveEmail)
				return EmailStatus.EmailDisabled;

			var operation = PlainText.Site["general", config.Language, operationType.ToString()];

			var accountInName = getAccountName(move.AccIn()) ?? "---";
			var accountOutName = getAccountName(move.AccOut()) ?? "---";

			var categoryName = move.Category?.Name ?? "---";
			var nature = PlainText.Site["general", config.Language, move.Nature.ToString()];

			var format = Format.MoveNotification(config.Language, config.Theme.Simplify());

			var dic = new Dictionary<String, String>
			{
				{ "Url", Dfm.Url },
				{ "Operation", operation },
				{ "AccountIn", accountInName },
				{ "AccountOut", accountOutName },
				{ "Date", move.Date.ToShortDateString() },
				{ "Nature", nature },
				{ "Category", categoryName },
				{ "Description", move.Description },
				{ "Value", move.Total().ToMoney(config.Language) },
				{ "Details", detailsHTML(move) },
			};

			var fileContent =
				format.Layout.Format(dic);

			try
			{
				new Sender()
					.To(user.Email)
					.Subject(format.Subject)
					.Body(fileContent)
					.Send();

				return EmailStatus.EmailSent;
			}
			catch (DFMEmailException e)
			{
				return e.Type;
			}
		}

		internal new Move GetNonCached(int id)
		{
			return base.GetNonCached(id);
		}

		private String detailsHTML(Move move)
		{
			if (!move.DetailList.Any())
				return null;

			var details = new StringBuilder();
			var config = GetUser(move).Config;
			var language = config.Language;

			foreach (var detail in move.DetailList)
			{
				var email = Format.FormatEmail
				(
					config.Theme.Simplify(),
					EmailType.Detail,
					new Dictionary<String, object> {
						{ "Description", detail.Description },
						{ "Amount", detail.Amount },
						{ "Value", detail.Value.ToMoney(language) },
						{ "Total", detail.GetTotal().ToMoney(language) }
					}
				);

				details.AppendLine(email);
			}

			return details.ToString();
		}

		private static String getAccountName(Account account)
		{
			return account?.Name;
		}
		#endregion

		internal Decimal GetIn(Month month, Category category)
		{
			var query = NewQuery().SimpleFilter(
				m => m.In != null && m.In.ID == month.ID
			);

			return get(query, category);
		}

		internal Decimal GetOut(Month month, Category category)
		{
			var query = NewQuery().SimpleFilter(
				m => m.Out != null && m.Out.ID == month.ID
			);

			return get(query, category);
		}

		private Decimal get(IQuery<Move> query, Category category)
		{
			query = category == null
				? query.SimpleFilter(m => m.Category == null)
				: query.SimpleFilter(m => m.Category != null && m.Category.ID == category.ID);

			return query.Result.Sum(m => m.Total());
		}

		internal override User GetUser(Move move)
		{
			if (move.ID != 0)
				move = Get(move.ID) ?? move;

			var month = move.Out ?? move.In;
			return month?.User();
		}
	}
}
