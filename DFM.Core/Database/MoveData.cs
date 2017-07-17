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
        }

        private void testDetailList(Move move)
        {
            if (!move.DetailList.Any())
                throw new CoreValidationException("At least one value required.");
        }

        private void testNature(Move move)
        {
            var hasIn = move.In != null;
            var hasOut = move.Out != null;

            switch (move.Nature)
            {
                case MoveNature.In:
                    if (!hasIn || hasOut) throw new CoreValidationException("An In move need to have an In Account, and can't have an Out Account.");
                    break;

                case MoveNature.Out: 
                    if (hasIn || !hasOut) throw new CoreValidationException("An Out move need to have an Out Account, and can't have an In Account.");
                    break;

                case MoveNature.Transfer:
                    if (!hasIn || !hasOut) throw new CoreValidationException("A Transfer move need to have Out and In Accounts.");
                    break;

            }
        }
    }
}
