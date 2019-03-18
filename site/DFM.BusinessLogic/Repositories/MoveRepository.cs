using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Multilanguage;
using DFM.Multilanguage.Emails;
using DFM.Multilanguage.Helpers;
using DK.NHibernate.Queries;

namespace DFM.BusinessLogic.Repositories
{
	internal class MoveRepository : GenericMoveRepository<Move>
	{
		internal Move SaveOrUpdate(Move move)
		{
			//Keep this order, weird errors happen if invert
			return SaveOrUpdate(move, validate, Complete);
		}

		private static void validate(Move move)
		{
			Validate(move);
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
			var user = move.User;
			var config = user.Config;

			if (!config.SendMoveEmail)
				return EmailStatus.EmailDisabled;

			var operation = PlainText.Dictionary["general", config.Language, operationType.ToString()];

			var accountInName = getAccountName(move.AccIn()) ?? "---";
			var accountOutName = getAccountName(move.AccOut()) ?? "---";

			var categoryName = move.Category?.Name ?? "---";
			var nature = PlainText.Dictionary["general", config.Language, move.Nature.ToString()];

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

		private static String detailsHTML(Move move)
		{
			if (!move.DetailList.Any())
				return null;

			var details = new StringBuilder();
			var config = move.User.Config;
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
						{ "Total", detail.GetTotal() }
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



	}
}
