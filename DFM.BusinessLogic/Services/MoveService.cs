using System;
using System.Collections.Generic;
using System.Text;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class MoveService : BaseMoveService<Move>
    {
        internal MoveService(IRepository<Move> repository) : base(repository) { }



        #region PlaceAccountsInMove
        internal void PlaceMonthsInMove(Move move, Month monthOut, Month monthIn)
        {
            if (monthOut != null && !monthOut.OutContains(move))
                monthOut.AddOut(move);
            else
                move.Out = null;

            if (monthIn != null && !monthIn.InContains(move))
                monthIn.AddIn(move);
            else
                move.In = null;

        }
        #endregion



        #region SendEmail
        internal void SendEmail(Move move, String action)
        {
            var user = move.User();

            if (!user.SendMoveEmail) return;

            var accountInName = accountName(move.AccIn());
            var accountOutName = accountName(move.AccOut());

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
            catch (Sender.DFMEmailException)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }
        }

        // ReSharper disable SuggestBaseTypeForParameter
        private static String detailsHTML(Move move)
        // ReSharper restore SuggestBaseTypeForParameter
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

        private static String accountName(Account account)
        {
            return account == null
                       ? null
                       : account.Name;
        }
        #endregion

    }
}
