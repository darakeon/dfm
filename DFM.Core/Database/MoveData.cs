using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.Core.Helpers;
using NHibernate.Linq;

namespace DFM.Core.Database
{
    public class MoveData : BaseData<Move>
    {
        TransferData transferData = new TransferData();

        public MoveData()
        {
            Validate += validate;
            Complete += complete;
        }

        private void complete(Move move)
        {
            ajustDetail(move);
            editTransfer(move);
        }

        private void editTransfer(Move move)
        {
            //var isEditing = move.ID != 0;
            //var isTransfer = move.Transfer != null;

            //if (isEditing && isTransfer)
            //{
            //    switch (move.Nature)
            //    {
            //        case MoveNature.In:
            //            move.Transfer.Out.Mirror(move);
            //            break;
            //        case MoveNature.Out:
            //            move.Transfer.In.Mirror(move);
            //            break;
            //    }
            //}
        }

        private void ajustDetail(Move move)
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
            if (move.Nature == MoveNature.Transfer)
                throw new CoreValidationException("If the move is a Transfer, call TestAndMakeTransfer before save.");

            if (!move.DetailList.Any())
                throw new CoreValidationException("At least one value required.");
        }



        public void TestAndMakeTransfer(Move move, Account accountToTransfer)
        {
            if (move.Nature == MoveNature.Transfer)
            {
                if (accountToTransfer == null)
                    throw new CoreValidationException("Another Account is required to create a Transfer Move.");

                transferData.Delete(move.Transfer);

                move.Transfer = new Transfer(move, accountToTransfer);
            }
        }



    }
}
