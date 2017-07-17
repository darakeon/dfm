using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class MoveData : BaseData<Move>
    {
        public MoveData()
        {
            Validate += validate;
            Complete += complete;
        }



        private void complete(Move move)
        {
            ajustDetailList(move);
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
            var summaryData = new SummaryData();
            var oldMove = SelectById(move.ID) ?? new Move();
            var category = move.Category;

            removeFromMonth(oldMove);

            summaryData.AjustMonth(move.In, oldMove.In, category);
            summaryData.AjustMonth(move.Out, oldMove.Out, category);

            summaryData.AjustYear(move.In, oldMove.In, category);
            summaryData.AjustYear(move.Out, oldMove.Out, category);
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



        public void PlaceAccountsInMove(Move move, Account currentAccount, Account otherAccount)
        {
            var yearData = new YearData();
            var monthData = new MonthData();

            var currentYear = yearData.GetYear(currentAccount, move.Date.Year);
            var currentMonth = monthData.GetMonth(currentYear, move.Date.Month);


            switch (move.Nature)
            {
                case MoveNature.Out:
                    currentMonth.AddOut(move);
                    break;
                case MoveNature.In:
                    currentMonth.AddIn(move);
                    break;
                case MoveNature.Transfer:
                    if (otherAccount == null)
                        throw new CoreValidationException("TransferMoveWrong");

                    currentMonth.AddOut(move);

                    var otherYear = yearData.GetYear(otherAccount, move.Date.Year);
                    var otherMonth = monthData.GetMonth(otherYear, move.Date.Month);

                    otherMonth.AddIn(move);
                    break;
                default:
                    throw new CoreValidationException("MoveNatureNotFound");
            }
        }



        public override void Delete(Move move)
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
