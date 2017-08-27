using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Bases
{
    public class GenericMoveRepository<T> : BaseRepository<T>
        where T : IEntity, IMove
    {
        internal GenericMoveRepository(IData<T> repository) : base(repository) { }



        #region Validate
        protected static void Validate(T move)
        {
            testDescription(move);
            testDate(move);
            testValue(move);
            testNature(move);
            testDetailList(move);
            testAccounts(move);
        }

        private static void testDescription(T move)
        {
            if (String.IsNullOrEmpty(move.Description))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDescriptionRequired);
        }

        private static void testDate(T move)
        {
            if (move.Date == DateTime.MinValue)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateRequired);

            var now =
                move.User == null
                    ? DateTime.UtcNow
                    : move.User.Now();

            if (typeof(T) != typeof(Schedule) && move.Date > now)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateInvalid);
        }

        private static void testValue(T move)
        {
            // TODO: When create value move, use the other if
            if (!move.IsDetailed() && move.Value() == 0)
            // if (!baseMove.IsDetailed() && baseMove.Value == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }

        private static void testNature(T move)
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

        private static void testDetailList(T move)
        {
            if (!move.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }

        private static void testAccounts(T move)
        {
            var moveInClosed = move.AccIn() != null && !move.AccIn().IsOpen();
            var moveOutClosed = move.AccOut() != null && !move.AccOut().IsOpen();

            if (moveInClosed || moveOutClosed)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (move.AccIn() != null && move.AccOut() != null && move.AccIn().ID == move.AccOut().ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
        }

        protected static void TestCategory(T move)
        {
            if (move.User.Config.UseCategories)
            {
                if (move.Category == null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

                if (!move.Category.Active)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
            }
            else
            {
                if (move.Category != null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoriesDisabled);
            }
        }
        #endregion

        #region Complete
        protected static void Complete(T move)
        {
            ajustDetailList(move);
        }

        private static void ajustDetailList(T move)
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
