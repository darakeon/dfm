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
        public MoveData()
        {
            Validate += validate;
            Complete += complete;
        }



        public new Move SaveOrUpdate(Move move)
        {
            throw new DFMCoreException("AccountMissing");
        }

        public Move SaveOrUpdate(Move move, Account account, Account secondAccount = null)
        {
            placeAccountsInMove(move, account, secondAccount);
            return base.SaveOrUpdate(move);
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
        private void complete(Move move)
        {
            ajustDetailList(move);

            if (move.ID != 0)
                ajustMonthAndYear(move);
        }

        private static void ajustDetailList(Move move)
        {
            if (move.DetailList.Count == 1 && move.DetailList[0].Description == null)
                move.DetailList[0].Description = move.Description;

            foreach (var detail in move.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;

                if (detail.Move == null)
                    detail.Move = move;
            }
        }

        private void ajustMonthAndYear(Move move)
        {
            invalidateSummary(move);
            
            var oldMove = SelectById(move.ID);

            if (oldMove != null)
                invalidateSummary(oldMove);
        }
        
        private static void invalidateSummary(Move move)
        {
            var summaryData = new SummaryData();

            if (move.Nature.In(MoveNature.In, MoveNature.Transfer))
                summaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountIn);
            
            if (move.Nature.In(MoveNature.Out, MoveNature.Transfer))
                summaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountOut);
        }
        #endregion



        private static void placeAccountsInMove(Move move, Account account, Account secondAccount = null)
        {
            var month = getMonth(move, account);
            var secondMonth = secondAccount == null ? null : getMonth(move, secondAccount);

            placeMonthsInMove(move, month, secondMonth);
        }

        private static Month getMonth(Move move, Account account)
        {
            var yearData = new YearData();
            var monthData = new MonthData();

            var year = yearData.GetOrCreateYear(move.Date.Year, account, move.Category);
            return monthData.GetOrCreateMonth(move.Date.Month, year, move.Category);
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



        public new void Delete(Move move)
        {
            removeFromMonth(move);
            ajustMonthAndYear(move);

            base.Delete(move);
        }

        private static void removeFromMonth(Move move)
        {
            if (move == null) return;

            var monthData = new MonthData();

            if (move.In != null)
            {
                move.In.InList.Remove(move);
                monthData.SaveOrUpdate(move.In);
            }

            if (move.Out != null)
            {
                move.Out.OutList.Remove(move);
                monthData.SaveOrUpdate(move.Out);
            }
        }


        public void Schedule(Move move, Account account, Account secondAccount, Scheduler scheduler)
        {
            for(var t = 0; t <= scheduler.Times; t++)
            {
                var newMove = move.Clone();

                switch (scheduler.Frequency)
                {
                    case ScheduleFrequency.Monthly:
                        newMove.Date = newMove.Date.AddMonths(t);
                        break;

                    case ScheduleFrequency.Yearly:
                        newMove.Date = newMove.Date.AddYears(t);
                        break;
                }

                SaveOrUpdate(newMove, account, secondAccount);
            }
        }
    }
}
