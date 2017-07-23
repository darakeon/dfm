using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;

namespace DFM.BusinessLogic.SuperServices
{
    public class MoneyService : BaseSuperService
    {
        private readonly MoveService moveService;
        private readonly DetailService detailService;
        private readonly MonthService monthService;
        private readonly ScheduleService scheduleService;

        internal MoneyService(ServiceAccess serviceAccess, MoveService moveService, DetailService detailService, MonthService monthService, ScheduleService scheduleService)
            : base(serviceAccess)
        {
            this.moveService = moveService;
            this.detailService = detailService;
            this.monthService = monthService;
            this.scheduleService = scheduleService;
        }


        public Move SelectMoveById(Int32 id)
        {
            var move = moveService.SelectById(id);

            if (move == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMove);

            return move;
        }



        public Move SaveOrUpdateMove(Move move, User user, String accountOutName, String accountInName, Category category)
        {
            BeginTransaction();
            
            try
            {
                move = Parent.BaseMove.SaveOrUpdateMove(move, user, accountOutName, accountInName, category);

                CommitTransaction();
            }
            catch
            {
                move.ID = 0;

                RollbackTransaction();
                throw;
            }


            return move;
        }



        public void DeleteMove(Int32 id)
        {
            BeginTransaction();

            try
            {
                var move = SelectMoveById(id);

                monthService.RemoveMoveFromMonth(move);
                Parent.BaseMove.AjustSummaries(move);

                moveService.Delete(id);

                moveService.SendEmail(move, "delete");

                if (move.Schedule != null)
                {
                    move.Schedule.Times--;
                    scheduleService.SaveOrUpdate(move.Schedule);
                }

                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }

        }



        public Detail SelectDetailById(Int32 id)
        {
            var detail = detailService.SelectById(id);

            if (detail == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidDetail);

            return detail;
        }


    }
}
