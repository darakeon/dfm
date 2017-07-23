using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Bases
{
    public class BaseMoveService<T> : BaseService<T> where T : BaseMove
    {
        internal BaseMoveService(IRepository<T> repository) : base(repository) { }


        internal T SaveOrUpdate(T move)
        {
            //Keep this order, weird errors happen if invert
            return SaveOrUpdate(move, validate, complete);
        }

        #region Validate
        private static void validate(BaseMove baseMove)
        {
            testDescription(baseMove);
            testDate(baseMove);
            testValue(baseMove);
            testNature(baseMove);
            testDetailList(baseMove);
            testAccounts(baseMove);
            testCategory(baseMove);
        }

        private static void testDescription(BaseMove baseMove)
        {
            if (String.IsNullOrEmpty(baseMove.Description))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDescriptionRequired);
        }

        private static void testDate(BaseMove baseMove)
        {
            if (baseMove.Date == DateTime.MinValue)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateRequired);

            if (baseMove.Date > DateTime.Now 
                    && baseMove.GetType() != typeof(FutureMove))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveDateInvalid);
        }

        private static void testValue(BaseMove baseMove)
        {
            // TODO: When create value move move, use the other if
            if (!baseMove.IsDetailed() && baseMove.Value() == 0)
            // if (!baseMove.IsDetailed() && baseMove.Value == 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }

        private static void testNature(BaseMove baseMove)
        {
            var hasIn = baseMove.AccIn() != null;
            var hasOut = baseMove.AccOut() != null;

            switch (baseMove.Nature)
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

        private static void testDetailList(BaseMove baseMove)
        {
            if (!baseMove.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveValueOrDetailRequired);
        }
        
        private static void testAccounts(BaseMove baseMove)
        {
            var moveInClosed = baseMove.AccIn() != null && !baseMove.AccIn().IsOpen();
            var moveOutClosed = baseMove.AccOut() != null && !baseMove.AccOut().IsOpen();

            if (moveInClosed || moveOutClosed)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (baseMove.AccIn() != null && baseMove.AccOut() != null && baseMove.AccIn().ID == baseMove.AccOut().ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
        }

        private static void testCategory(BaseMove baseMove)
        {
            if (baseMove.Category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            if (!baseMove.Category.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
        }
        #endregion

        #region Complete
        private static void complete(BaseMove baseMove)
        {
            ajustDetailList(baseMove);
        }
        
        private static void ajustDetailList(BaseMove baseMove)
        {
            if (!baseMove.IsDetailed())
            {
                baseMove.DetailList[0].Description = baseMove.Description;
                baseMove.DetailList[0].Amount = 1;
            }

            foreach (var detail in baseMove.DetailList
                                    .Where(detail => detail.Value < 0))
            {
                detail.Value = -detail.Value;
            }
        }

        #endregion

        internal void Delete(Int32 id)
        {
            var move = SelectById(id);

            Delete(move);
        }


    }
}
