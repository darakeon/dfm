using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
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
            //Keep inverted, weird errors happen if make in the right order
            return SaveOrUpdate(move, Validate, Complete);
        }




        #region Validate
        protected static void Validate(BaseMove baseMove)
        {
            testDetailList(baseMove);
            testNature(baseMove);
            testAccounts(baseMove);
            testCategory(baseMove);
        }

        private static void testDetailList(BaseMove baseMove)
        {
            if (!baseMove.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailRequired);
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
        
        private static void testAccounts(BaseMove baseMove)
        {
            var moveInClosed = baseMove.AccIn() != null && !baseMove.AccIn().Open();
            var moveOutClosed = baseMove.AccOut() != null && !baseMove.AccOut().Open();

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
        protected static void Complete(BaseMove baseMove)
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

            foreach (var detail in baseMove.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;
            }
        }
        #endregion



    }
}
