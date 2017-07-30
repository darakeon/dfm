using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Bases
{
    public class BaseMoveService : BaseService<Move>
    {
        internal BaseMoveService(IRepository<Move> repository) : base(repository) { }


        internal Move SaveOrUpdate(Move move)
        {
            //Keep this order, weird errors happen if invert
            return SaveOrUpdate(move, validate, complete);
        }

        #region Validate
        private static void validate(Move move)
        {
            testDescription(move);
            testDate(move);
            testValue(move);
            testNature(move);
            testDetailList(move);
            testAccounts(move);
            testCategory(move);
        }

        private static void testDescription(Move move)
        {
            if (String.IsNullOrEmpty(move.Description))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDescriptionRequired);
        }

        private static void testDate(Move move)
        {
            if (move.Date == DateTime.MinValue)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateRequired);

            if (move.Date > DateTime.Now)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateInvalid);
        }

        private static void testValue(Move move)
        {
            // TODO: When create value move, use the other if
            if (!move.IsDetailed() && move.Value() == 0)
            // if (!baseMove.IsDetailed() && baseMove.Value == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }

        private static void testNature(Move move)
        {
            var hasIn = move.AccIn() != null;
            var hasOut = move.AccOut() != null;

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

        private static void testDetailList(Move move)
        {
            if (!move.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }

        private static void testAccounts(Move move)
        {
            var moveInClosed = move.AccIn() != null && !move.AccIn().IsOpen();
            var moveOutClosed = move.AccOut() != null && !move.AccOut().IsOpen();

            if (moveInClosed || moveOutClosed)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (move.AccIn() != null && move.AccOut() != null && move.AccIn().ID == move.AccOut().ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
        }

        private static void testCategory(Move move)
        {
            if (move.Category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            if (!move.Category.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
        }
        #endregion

        #region Complete
        private static void complete(Move move)
        {
            ajustDetailList(move);
        }

        private static void ajustDetailList(Move move)
        {
            if (!move.IsDetailed())
            {
                move.DetailList[0].Description = move.Description;
                move.DetailList[0].Amount = 1;
            }

            foreach (var detail in move.DetailList
                                    .Where(detail => detail.Value < 0))
            {
                detail.Value = -detail.Value;
            }
        }

        #endregion


    }
}
