using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ak.Generic.Enums;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class MoveService : BaseService<Move>
    {
        internal MoveService(IRepository<Move> repository) : base(repository) { }

        

        internal Move SaveOrUpdate(Move move)
        {
            //Keep inverted, weird errors happen if make in the right order
            return SaveOrUpdate(move, validate, complete);
        }



        #region Complete
        private static void complete(Move move)
        {
            ajustDetailList(move);
        }

        private static void ajustDetailList(Move move)
        {
            if (move.DetailList.Count == 1 && move.DetailList[0].Description == null)
            {
                move.DetailList[0].Description = move.Description;
                move.DetailList[0].Amount = 1;
            }

            foreach (var detail in move.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;

                if (detail.Move == null)
                    detail.Move = move;
            }
        }




        #endregion



        #region Validate
        private static void validate(Move move)
        {
            testDetailList(move);
            testNature(move);
            testAccounts(move);
            testCategory(move);
            testDate(move);
        }

        private static void testDetailList(Move move)
        {
            if (!move.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailRequired);
        }

        private static void testNature(Move move)
        {
            var hasIn = move.In != null;
            var hasOut = move.Out != null;

            switch (move.Nature)
            {
                case MoveNature.In:
                    if (!hasIn || hasOut)
                        throw DFMCoreException.WithMessage(ExceptionPossibilities.InMoveWrong);
                    break;

                case MoveNature.Out:
                    if (hasIn || !hasOut)
                        throw DFMCoreException.WithMessage(ExceptionPossibilities.OutMoveWrong);
                    break;

                case MoveNature.Transfer:
                    if (!hasIn || !hasOut)
                        throw DFMCoreException.WithMessage(ExceptionPossibilities.TransferMoveWrong);
                    break;

            }
        }

        private static void testAccounts(Move move)
        {
            var moveInClosed = move.In != null && !move.In.Year.Account.Open();
            var moveOutClosed = move.Out != null && !move.Out.Year.Account.Open();

            if (moveInClosed || moveOutClosed)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (move.In != null && move.Out != null && move.In.Year.Account == move.Out.Year.Account)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
        }

        private static void testCategory(Move move)
        {
            if (move.Category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            if (!move.Category.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
        }

        private static void testDate(Move move)
        {
            var isFutureMove = move.Date > DateTime.Today;

            var isFirstOfSchedule = move.Schedule != null
                                    && move.Schedule.IsFirstMove();

            if (isFutureMove && !isFirstOfSchedule)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveFutureNotScheduled);
        }
        #endregion



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






        internal void SendEmail(Move move, Format.GetterForMove getterForMove, String action)
        {
            if (!move.User().SendMoveEmail) return;

            var accountInName = accountName(move.AccountIn());
            var accountOutName = accountName(move.AccountOut());

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

    }
}
