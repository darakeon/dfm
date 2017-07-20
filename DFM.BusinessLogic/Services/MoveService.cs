using System;
using System.Collections.Generic;
using System.Text;
using Ak.Generic.Enums;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class MoveService : BaseMoveService<Move>
    {
        internal MoveService(IRepository<Move> repository) : base(repository) { }



        #region PlaceAccountsInMove
        internal void PlaceMonthsInMove(Move move, Month monthOut, Month monthIn)
        {
            var errorMessage = String.Format("{0}MoveWrong", move.Nature);
            var errorEnumValue = Str2Enum.Cast<ExceptionPossibilities>(errorMessage);

            if (move.Nature != MoveNature.In)
            {
                if (monthOut == null) throw DFMCoreException.WithMessage(errorEnumValue);
                if (!monthOut.OutContains(move)) monthOut.AddOut(move);
            }
            else
            {
                move.Out = null;
            }

            if (move.Nature != MoveNature.Out)
            {
                if (monthIn == null) throw DFMCoreException.WithMessage(errorEnumValue);
                if (!monthIn.InContains(move)) monthIn.AddIn(move);
            }
            else
            {
                move.In = null;
            }

        }
        #endregion



        #region SendEmail
        internal void SendEmail(Move move, Format.GetterForMove getterForMove, String action)
        {
            if (!move.User().SendMoveEmail) return;

            var accountInName = accountName(move.AccIn());
            var accountOutName = accountName(move.AccOut());

            var format = getterForMove(move.Nature);

            var dic = new Dictionary<String, String>
                            {
                                { "Url", Dfm.Url },
                                { "AccountIn", accountInName },
                                { "AccountOut", accountOutName },
                                { "Date", move.Date.ToShortDateString() },
                                { "Category", move.Category.Name },
                                { "Description", move.Description },
                                { "Value", move.Value().ToMoney() },
                                { "Details", detailsHTML(move) },
                                { "Action", action }
                            };

            var fileContent =
                format.Layout.Format(dic);

            try
            {
                new Sender()
                    .To(move.User().Email)
                    .Subject(format.Subject)
                    .Body(fileContent)
                    .Send();
            }
            catch (Exception)
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
            }
        }

        private static String detailsHTML(Move move)
        {
            var details = new StringBuilder();

            foreach (var detail in move.DetailList)
            {
                details.Append(
                    String.Format(
                        "{0}: {1} x {2}<br />"
                        , detail.Description
                        , detail.Amount
                        , detail.Value.ToMoney()));
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
