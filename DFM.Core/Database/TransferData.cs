using System;
using DFM.Core.Entities;
using DFM.Core.Enums;

namespace DFM.Core.Database
{
    public class TransferData : BaseData<Transfer>
    {
        public TransferData()
        {
            Validate += validate;
        }

        private void validate(Transfer transfer)
        {
            var movesIsRight = transfer.In.Nature == MoveNature.In
                               && transfer.Out.Nature == MoveNature.Out;

            if (!movesIsRight)
            {
                throw new ApplicationException("Moves are with wrong Nature.");
            }
        }
    }
}
