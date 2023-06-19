using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Keon.Util.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Language;
using DFM.Language.Emails;
using DFM.Language.Extensions;
using Keon.NHibernate.Queries;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class MoveRepository : GenericMoveRepository<Move>
	{
		protected override Int32 descriptionMaxSize => MaxLen.MoveDescription;
		protected override Error descriptionError => Error.TooLargeMoveDescription;

		private readonly Current.GetUrl getUrl;

		public MoveRepository(Current.GetUrl getUrl)
		{
			this.getUrl = getUrl;
		}

		internal Move Get(Guid guid)
		{
			return SingleOrDefault(m => m.ExternalId == guid.ToByteArray());
		}

		internal Move SaveMainInfo(Move move, DateTime now)
		{
			//Keep this order, weird errors happen if invert
			return SaveOrUpdate(
				move,
				m => validate(m, now),
				complete
			);
		}

		internal Move SaveCheck(Move move)
		{
			return SaveOrUpdate(move);
		}

		#region SendEmail
		internal EmailStatus SendEmail(Move move, OperationType operationType, Security security)
		{
			var user = GetUser(move);
			var settings = user.Settings;

			if (!settings.SendMoveEmail)
				return EmailStatus.EmailDisabled;

			var operation = PlainText.Site[
				"general", settings.Language, operationType.ToString()
			].Text;

			var accountInName = move.In?.Name ?? "---";
			var accountOutName = move.Out?.Name ?? "---";
			var categoryName = move.Category?.Name ?? "---";

			var nature = PlainText.Site[
				"general", settings.Language, move.Nature.ToString()
			].Text;

			var format = Format.MoveNotification(user);

			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Operation", operation },
				{ "AccountIn", accountInName },
				{ "AccountOut", accountOutName },
				{ "Date", move.GetDate().ToShortDateString() },
				{ "Nature", nature },
				{ "Category", categoryName },
				{ "Description", move.Description },
				{ "Value", move.Value.ToMoney(settings.Language) },
				{ "Details", detailsHTML(move) },
				{ "UnsubscribePath", security.Action.ToString() },
				{ "UnsubscribeToken", security.Token },
			};

			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(user.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
				return EmailStatus.EmailSent;
			}
			catch (MailError e)
			{
				return e.Type;
			}
		}

		internal Move GetNonCached(Guid guid)
		{
			return newNonCachedQuery(
				q => q.Where(m => m.ExternalId == guid.ToByteArray())
					.FirstOrDefault
			);
		}

		private String detailsHTML(Move move)
		{
			if (!move.DetailList.Any())
				return null;

			var details = new StringBuilder();
			var user = GetUser(move);
			var settings = user.Settings;
			var language = settings.Language;
			var misc = user.GenerateMisc();

			foreach (var detail in move.DetailList)
			{
				var email = Format.FormatEmail
				(
					settings.Theme,
					EmailType.Detail,
					null,
					new Dictionary<String, Object> {
						{ "Description", detail.Description },
						{ "Amount", detail.Amount },
						{ "Value", detail.Value.ToMoney(language) },
						{ "Total", detail.GetTotal().ToMoney(language) }
					},
					misc
				);

				details.AppendLine(email);
			}

			return details.ToString();
		}
		#endregion

		internal Decimal GetIn(Summary summary)
		{
			var query = NewQuery().Where(
				m => m.In != null && m.In.ID == summary.Account.ID
			);

			return get(query, summary);
		}

		internal Decimal GetOut(Summary summary)
		{
			var query = NewQuery().Where(
				m => m.Out != null && m.Out.ID == summary.Account.ID
			);

			return get(query, summary);
		}

		private Decimal get(Query<Move, Int64> query, Summary summary)
		{
			query = summary.Category == null
				? query.Where(m => m.Category == null)
				: query.Where(m => m.Category != null && m.Category.ID == summary.Category.ID);

			switch (summary.Nature)
			{
				case SummaryNature.Month:
					var year = summary.Time / 100;
					var month = summary.Time % 100;

					query = query.Where(m =>
						m.Month == month
						&& m.Year == year
					);

					break;
				case SummaryNature.Year:
					query = query.Where(m =>
						m.Year == summary.Time
					);

					break;
				default:
					throw new NotImplementedException();
			}

			// TODO: use summarize from query
			return query
				.List
				.Sum(m => m.Value);
		}

		internal override User GetUser(Move move)
		{
			if (move.ID != 0)
				move = Get(move.ID) ?? move;

			var account = move.Out ?? move.In;
			return account?.User;
		}

		public IList<Move> ByAccount(Account account)
		{
			return byAccount(account).List;
		}

		public Boolean AccountHasMoves(Account account)
		{
			return byAccount(account).Any;
		}

		public IList<Move> ByAccountAndTime(Account account, Int16 dateYear, Int16 dateMonth)
		{
			return byAccount(account)
				.Where(
					m => m.Year == dateYear
						&& m.Month == dateMonth
				)
				.OrderBy(m => m.Day)
				.List;
		}

		private Query<Move, Int64> byAccount(Account account)
		{
			return NewQuery()
				.Where(
					m => (m.In != null && m.In.ID == account.ID)
						|| (m.Out != null && m.Out.ID == account.ID)
				);
		}

		public IList<Move> ByDescription(String userEmail, params String[] terms)
		{
			return byDescription(userEmail, PrimalMoveNature.In, terms)
				.Union(byDescription(userEmail, PrimalMoveNature.Out, terms))
				.ToList();
		}

		private IList<Move> byDescription(String userEmail, PrimalMoveNature nature, params String[] terms)
		{
			var userRelation = getUserRelation(nature);

			var query = NewQuery()
				.Where(
					userRelation,
					User.Compare(userEmail)
				);

			terms.ToList().ForEach(
				term => query = query.Like(d => d.Description, term)
			);

			return query.List;
		}

		private static Expression<Func<Move, User>> getUserRelation(PrimalMoveNature nature)
		{
			switch (nature)
			{
				case PrimalMoveNature.In:
					return d => d.In.User;
				case PrimalMoveNature.Out:
					return d => d.Out.User;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public IList<Move> ByCategory(Category category)
		{
			return Where(m => m.Category.ID == category.ID);
		}
	}
}
