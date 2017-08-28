using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DK.Generic.Extensions;
using DK.NHibernate.Base;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Multilanguage.Helpers;

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
        internal EmailStatus SendEmail(Move move)
        {
            var user = move.User;

            if (!user.Config.SendMoveEmail)
                return EmailStatus.EmailDisabled;

            var accountInName = getAccountName(move.AccIn());
            var accountOutName = getAccountName(move.AccOut());

            var categoryName = move.Category == null
                ? "---"
                : move.Category.Name;

            var format = new Format(user.Config.Language, move.Nature);

            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "AccountIn", accountInName },
                                { "AccountOut", accountOutName },
                                { "Date", move.Date.ToShortDateString() },
                                { "Category", categoryName },
                                { "Description", move.Description },
                                { "Value", move.Total().ToMoney(user.Config.Language) },
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

        private static String detailsHTML(Move move)
        {
            var details = new StringBuilder();
            var language = move.User.Config.Language;

            foreach (var detail in move.DetailList)
            {
                details.Append(
                    String.Format(
                        "{0}: {1} x {2}<br />"
                        , detail.Description
                        , detail.Amount
                        , detail.Value.ToMoney(language)));
            }

            return details.ToString();
        }

        private static String getAccountName(Account account)
        {
            return account == null
                ? null : account.Name;
        }
        #endregion



        internal Decimal GetIn(Month month, Category category)
		{
			var query = NewQuery().Filter(m => m.In.ID == month.ID);

			return get(query, category);
		}

        internal Decimal GetOut(Month month, Category category)
		{
			var query = NewQuery().Filter(m => m.Out.ID == month.ID);

			return get(query, category);
		}

        private Decimal get(Query<Move> query, Category category)
		{
			query = category == null
				? query.Filter(m => m.Category == null)
				: query.Filter(m => m.Category != null && m.Category.ID == category.ID);

			return query.Result.Sum(m => m.Total());
		}



    }
}
