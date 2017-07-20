using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        protected static void Validate(BaseMove move)
        {
            testDetailList(move);
            testNature(move);
            testAccounts(move);
            testCategory(move);
            testDate(move);
        }

        private static void testDetailList(BaseMove move)
        {
            if (!move.DetailList.Any())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DetailRequired);
        }

        private static void testNature(BaseMove move)
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

        private static void testAccounts(BaseMove move)
        {
            var moveInClosed = move.AccIn() != null && !move.AccIn().Open();
            var moveOutClosed = move.AccOut() != null && !move.AccOut().Open();

            if (moveInClosed || moveOutClosed)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (move.AccIn() != null && move.AccOut() != null && move.AccIn().ID == move.AccOut().ID)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveCircularTransfer);
        }

        private static void testCategory(BaseMove move)
        {
            if (move.Category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            if (!move.Category.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledCategory);
        }

        private static void testDate(BaseMove move)
        {
            var isFutureMove = move.Date > DateTime.Today;

            var isFirstOfSchedule = move.Schedule != null
                                    && move.Schedule.IsFirstMove();

            if (isFutureMove && !isFirstOfSchedule)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.MoveFutureNotScheduled);
        }
        #endregion



        #region Complete
        protected static void Complete(BaseMove move)
        {
            ajustDetailList(move);
        }

        private static void ajustDetailList(BaseMove move)
        {
            if (!move.IsDetailed())
            {
                move.DetailList[0].Description = move.Description;
                move.DetailList[0].Amount = 1;
            }

            foreach (var detail in move.DetailList)
            {
                if (detail.Value < 0)
                    detail.Value = -detail.Value;
            }
        }
        #endregion



    }
}
