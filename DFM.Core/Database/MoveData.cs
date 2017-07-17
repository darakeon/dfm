using System;
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
            if (move.DetailList.Count == 1 && move.DetailList[0].Description == null)
            {
                move.DetailList[0].Description = move.Description;
            }

            foreach (var detail in move.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;

                if (detail.Move == null)
                    detail.Move = move;
            }
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
            if (move.In == move.Out)
                throw new CoreValidationException("CircularTransfer");

            var moveInClosed = move.In != null && !move.In.Open;
            var moveOutClosed = move.Out != null && !move.Out.Open;

            if (moveInClosed || moveOutClosed)
                throw new CoreValidationException("ClosedAccount");
        }

        private void testCategory(Move move)
        {
            if (!move.Category.Active)
                throw new CoreValidationException("DisabledCategory");
        }

    }
}
