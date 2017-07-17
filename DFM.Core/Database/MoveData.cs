using System.Linq;
using DFM.Core.Database.Bases;
using DFM.Core.Entities;
using DFM.Core.Enums;
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
            throw new CoreValidationException("AccountMissing");
        }

        public Move SaveOrUpdate(Move move, Account account, Account secondAccount = null)
        {
            placeAccountsInMove(move, account, secondAccount);
            return base.SaveOrUpdate(move);
        }



        private void validate(Move move)
        {
            testDetailList(move);
            testNature(move);
            testAccounts(move);
            testCategory(move);
        }

        private void testDetailList(Move move)
        {
            if (!move.DetailList.Any())
                throw new CoreValidationException("DetailRequired");
        }

        private void testNature(Move move)
        {
            var hasIn = move.In != null;
            var hasOut = move.Out != null;

            switch (move.Nature)
            {
                case MoveNature.In:
                    if (!hasIn || hasOut)
                        throw new CoreValidationException("InMoveWrong");
                    break;

                case MoveNature.Out:
                    if (hasIn || !hasOut)
                        throw new CoreValidationException("OutMoveWrong");
                    break;

                case MoveNature.Transfer:
                    if (!hasIn || !hasOut)
                        throw new CoreValidationException("TransferMoveWrong");
                    break;

            }
        }

        private void testAccounts(Move move)
        {
            var moveInClosed = move.In != null && !move.In.Year.Account.Open;
            var moveOutClosed = move.Out != null && !move.Out.Year.Account.Open;

            if (moveInClosed || moveOutClosed)
                throw new CoreValidationException("ClosedAccount");

            if (move.In != null && move.Out != null && move.In.Year.Account == move.Out.Year.Account)
                throw new CoreValidationException("CircularTransfer");
        }

        private void testCategory(Move move)
        {
            if (!move.Category.Active)
                throw new CoreValidationException("DisabledCategory");
        }


        
        private void complete(Move move)
        {
            ajustDetailList(move);

            if (move.ID != 0)
                ajustMonthAndYear(move);
        }

        private void ajustDetailList(Move move)
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



        private void invalidateSummary(Move move)
        {
            var summaryData = new SummaryData();

            if (move.Nature.In(MoveNature.In, MoveNature.Transfer))
                summaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountIn);
            
            if (move.Nature.In(MoveNature.Out, MoveNature.Transfer))
                summaryData.Invalidate(move.Date.Month, move.Date.Year, move.Category, move.AccountOut);
        }



        private void placeAccountsInMove(Move move, Account account, Account secondAccount = null)
        {
            var yearData = new YearData();
            var monthData = new MonthData();

            var year = yearData.GetOrCreateYear(move.Date.Year, account, move.Category);
            var month = monthData.GetOrCreateMonth(move.Date.Month, year, move.Category);

            switch (move.Nature)
            {
                case MoveNature.Out:
                    month.AddOut(move); break;
                case MoveNature.In:
                    month.AddIn(move); break;
                case MoveNature.Transfer:
                    if (secondAccount == null)
                        throw new CoreValidationException("TransferMoveWrong");

                    month.AddOut(move);

                    var secondYear = yearData.GetOrCreateYear(move.Date.Year, secondAccount, move.Category);
                    var secondMonth = monthData.GetOrCreateMonth(move.Date.Month, secondYear, move.Category);

                    secondMonth.AddIn(move);

                    break;
                default:
                    throw new CoreValidationException("MoveNatureNotFound");
            }
        }



        public new void Delete(Move move)
        {
            removeFromMonth(move);
            ajustMonthAndYear(move);

            base.Delete(move);
        }

        private void removeFromMonth(Move move)
        {
            if (move == null) return;

            if (move.In != null) move.In.InList.Remove(move);
            if (move.Out != null) move.Out.OutList.Remove(move);
        }
    }
}
