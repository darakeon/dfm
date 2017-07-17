using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Enums;
using System;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class MoveData : BaseData<Move>
    {
        public Move SaveOrUpdate(Move move, Account accountToTransfer = null)
        {
            Validate(move);

            MakeTransfer(move, accountToTransfer);

            return base.SaveOrUpdate(move);
        }

        public void MakeTransfer(Move move, Account accountToTransfer)
        {
            if (move.Nature == MoveNature.Transfer)
            {
                if (accountToTransfer == null)
                    throw new CoreValidationException("Another Account is required to create a Transfer Move.");

                move.Transfer = new Transfer(move, accountToTransfer);
            }
        }

        public void Validate(Move move)
        {
            if (!move.DetailList.Any())
                throw new CoreValidationException("At least one value required.");


            foreach (var detail in move.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;

                if (detail.Move == null)
                    detail.Move = move;
            }
        }
    }
}
