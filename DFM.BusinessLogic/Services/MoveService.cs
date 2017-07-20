using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ak.Generic.Collection;
using Ak.Generic.Enums;
using Ak.Generic.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Core.Enums;
using DFM.Email;
using DFM.Entities;
using DFM.Extensions.Entities;

namespace DFM.BusinessLogic.Services
{
    public class MoveService : BaseService<Move>
    {
        internal MoveService(DataAccess father, IRepository repository) : base(father, repository) { }

        public Move SaveOrUpdate(Move move, Account accountOut, Account accountIn, Format.GetterForMove getterForMove)
        {
            var action = move.ID == 0 ? "create_move" : "edit";

            ajustOldSummaries(move.ID);

            placeAccountsInMove(move, accountOut, accountIn);
            move = saveOrUpdate(move);

            foreach (var detail in move.DetailList)
            {
                Father.Detail.SaveOrUpdate(detail);
            }

            ajustSummaries(move);

            sendEmail(move, getterForMove, action);

            return move;
        }

        private Move saveOrUpdate(Move move)
        {
            //Keep inverted, weird errors happen if make in the right order
            return SaveOrUpdate(move, validate, complete);
        }



        #region Complete
        private void complete(Move move)
        {
            ajustDetailList(move);
            ajustSchedule(move);
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


        private void ajustSchedule(Move move)
        {
            if (move.Schedule == null 
                || move.Schedule.ID != 0) return;

            if (!move.Schedule.Contains(move))
                move.Schedule.AddMove(move);

            Father.Schedule.SaveOrUpdate(move.Schedule);
        }


        private void ajustOldSummaries(Int32 id)
        {
            var oldMove = SelectOldById(id);

            if (oldMove == null) return;

            if (oldMove.In != null)
                oldMove.RemoveFromIn();

            if (oldMove.Out != null)
                oldMove.RemoveFromOut();

            ajustSummaries(oldMove);
        }

        private void ajustSummaries(Move move)
        {
            if (move.Nature.IsIn(MoveNature.In, MoveNature.Transfer))
                Father.Summary.Ajust((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccountIn());

            if (move.Nature.IsIn(MoveNature.Out, MoveNature.Transfer))
                Father.Summary.Ajust((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccountOut());
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
        private void placeAccountsInMove(Move move, Account accountOut, Account accountIn)
        {
            var monthOut = accountOut == null ? null : getMonth(move, accountOut);
            var monthIn = accountIn == null ? null : getMonth(move, accountIn);

            placeMonthsInMove(move, monthOut, monthIn);
        }

        private Month getMonth(Move move, Account account)
        {
            var year = Father.Year.GetOrCreateYear((Int16)move.Date.Year, account, move.Category);
            return Father.Month.GetOrCreateMonth((Int16)move.Date.Month, year, move.Category);
        }

        private static void placeMonthsInMove(Move move, Month monthOut, Month monthIn)
        {
            var errorMessage = String.Format("{0}MoveWrong", move.Nature);
            var errorEnumValue = Str2Enum.Cast<ExceptionPossibilities>(errorMessage);

            var error = DFMCoreException.WithMessage(errorEnumValue);

            if (move.Nature != MoveNature.In)
            {
                if (monthOut == null) throw error;
                if (!monthOut.OutContains(move)) monthOut.AddOut(move);
            }
            else
            {
                move.Out = null;
            }

            if (move.Nature != MoveNature.Out)
            {
                if (monthIn == null) throw error;
                if (!monthIn.InContains(move)) monthIn.AddIn(move);
            }
            else
            {
                move.In = null;
            }

        }
        #endregion



        public void Delete(Move move, Format.GetterForMove getterForMove)
        {
            removeFromMonth(move);
            ajustSummaries(move);

            Delete(move);

            sendEmail(move, getterForMove, "delete");
        }

        private void removeFromMonth(Move move)
        {
            if (move == null) return;

            if (move.In != null)
            {
                move.In.InList.Remove(move);
                Father.Month.SaveOrUpdate(move.In);
            }

            if (move.Out != null)
            {
                move.Out.OutList.Remove(move);
                Father.Month.SaveOrUpdate(move.Out);
            }
        }



        private static void sendEmail(Move move, Format.GetterForMove getterForMove, String action)
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
                //TO-DO: tell user that the e-mail was not send
                var exception = DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);

                new Sender()
                    .To(move.User().Email)
                    .Subject(format.Subject)
                    .Body(fileContent)
                    .Send(exception);
            }
            catch (DFMCoreException) { }
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
