using System;
using System.Linq;
using DFM.Core.Enums;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Helpers;
using Ak.Generic.Collection;

namespace DFM.Core.Database
{
    public class MoveData : BaseData<Move>
    {
		private MoveData() { }

        public static Move SaveOrUpdate(Move move, Account account, Account secondAccount = null)
        {
            placeAccountsInMove(move, account, secondAccount);
            
            return SaveOrUpdate(move, validate, complete);
        }



        #region Validate
        private static void validate(Move move)
        {
            testDetailList(move);
            testNature(move);
            testAccounts(move);
            testCategory(move);
        }

        private static void testDetailList(Move move)
        {
            if (!move.DetailList.Any())
                throw new DFMCoreException("DetailRequired");
        }

        private static void testNature(Move move)
        {
            var hasIn = move.In != null;
            var hasOut = move.Out != null;

            switch (move.Nature)
            {
                case MoveNature.In:
                    if (!hasIn || hasOut)
                        throw new DFMCoreException("InMoveWrong");
                    break;

                case MoveNature.Out:
                    if (hasIn || !hasOut)
                        throw new DFMCoreException("OutMoveWrong");
                    break;

                case MoveNature.Transfer:
                    if (!hasIn || !hasOut)
                        throw new DFMCoreException("TransferMoveWrong");
                    break;

            }
        }

        private static void testAccounts(Move move)
        {
            var moveInClosed = move.In != null && !move.In.Year.Account.Open;
            var moveOutClosed = move.Out != null && !move.Out.Year.Account.Open;

            if (moveInClosed || moveOutClosed)
                throw new DFMCoreException("ClosedAccount");

            if (move.In != null && move.Out != null && move.In.Year.Account == move.Out.Year.Account)
                throw new DFMCoreException("CircularTransfer");
        }

        private static void testCategory(Move move)
        {
            if (!move.Category.Active)
                throw new DFMCoreException("DisabledCategory");
        }
        #endregion



        #region Complete
        private static void complete(Move move)
        {
            ajustDetailList(move);

            if (move.ID != 0)
                ajustMonthAndYear(move);
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

        private static void ajustMonthAndYear(Move move)
        {
            invalidateSummary(move);
            
            var oldMove = SelectById(move.ID);

            if (oldMove != null)
                invalidateSummary(oldMove);
        }
        
        private static void invalidateSummary(Move move)
        {
            if (move.Nature.In(MoveNature.In, MoveNature.Transfer))
                SummaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountIn);
            
            if (move.Nature.In(MoveNature.Out, MoveNature.Transfer))
                SummaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountOut);
        }
        #endregion



        #region PlaceAccountsInMove
        private static void placeAccountsInMove(Move move, Account account, Account secondAccount = null)
        {
            var month = getMonth(move, account);
            var secondMonth = secondAccount == null ? null : getMonth(move, secondAccount);

            placeMonthsInMove(move, month, secondMonth);
        }

        private static Month getMonth(Move move, Account account)
        {
            var year = YearData.GetOrCreateYear(move.Date.Year, account, move.Category);
            return MonthData.GetOrCreateMonth(move.Date.Month, year, move.Category);
        }

        private static void placeMonthsInMove(Move move, Month month, Month secondMonth = null)
        {
            switch (move.Nature)
            {
                case MoveNature.Out:
                    month.AddOut(move); break;
                case MoveNature.In:
                    month.AddIn(move); break;
                case MoveNature.Transfer:
                    if (secondMonth == null)
                        throw new DFMCoreException("TransferMoveWrong");

                    month.AddOut(move);
                    secondMonth.AddIn(move);

                    break;
                default:
                    throw new DFMCoreException("MoveNatureNotFound");
            }
        }
        #endregion



        public static new void Delete(Move move)
        {
            removeFromMonth(move);
            ajustMonthAndYear(move);

            BaseData<Move>.Delete(move);
        }

        private static void removeFromMonth(Move move)
        {
            if (move == null) return;

            if (move.In != null)
            {
                move.In.InList.Remove(move);
                MonthData.SaveOrUpdate(move.In);
            }

            if (move.Out != null)
            {
                move.Out.OutList.Remove(move);
                MonthData.SaveOrUpdate(move.Out);
            }
        }



        public static void Schedule(Move move, Account account, Account secondAccount, Schedule schedule)
        {
            for(var t = 0; t <= schedule.Times; t++)
            {
                var newMove = move.Clone();

                switch (schedule.Frequency)
                {
                    case ScheduleFrequency.Monthly:
                        newMove.Date = newMove.Date.AddMonths(t);
                        break;

                    case ScheduleFrequency.Yearly:
                        newMove.Date = newMove.Date.AddYears(t);
                        break;
                }

                if (newMove.Date > DateTime.Now)
                    newMove.Scheduled = true;

                SaveOrUpdate(newMove, account, secondAccount);
            }
        }



        internal static void MakeVisible(Move move)
        {
            move.Scheduled = false;
            SaveOrUpdate(move, validate, complete);
        }
    }
}
