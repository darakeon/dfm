using System;
using System.Collections.Generic;
using System.Text;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Email.Exceptions;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Multilanguage.Helpers;
using ExceptionPossibilities = DFM.BusinessLogic.Exceptions.ExceptionPossibilities;

namespace DFM.BusinessLogic.Repositories
{
    internal class MoveRepository : GenericMoveRepository<Move>
    {
        internal MoveRepository(IData<Move> repository) : base(repository) { }


        internal Move SaveOrUpdate(Move move)
        {
            //Keep this order, weird errors happen if invert
            return SaveOrUpdate(move, Validate, Complete);
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
        internal void SendEmail(Move move, String action)
        {
            var user = move.User();

            if (!user.SendMoveEmail) return;

            var accountInName = getAccountName(move.AccIn());
            var accountOutName = getAccountName(move.AccOut());

            var format = new Format(user.Language, move.Nature);

            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "AccountIn", accountInName },
                                { "AccountOut", accountOutName },
                                { "Date", move.Date.ToShortDateString() },
                                { "Category", move.Category.Name },
                                { "Description", move.Description },
                                { "Value", move.Value().ToMoney(user.Language) },
                                { "Details", detailsHTML(move) },
                                { "Action", action }
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
            }
            catch (DFMEmailException)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }
        }

        private static String detailsHTML(Move move)
        {
            var details = new StringBuilder();
            var language = move.User().Language;

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


    }
}
