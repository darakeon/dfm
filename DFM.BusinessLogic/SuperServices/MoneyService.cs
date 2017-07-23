using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Generic;

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
            VerifyUser();

            var move = moveService.SelectById(id);

            VerifyMove(move);

            return move;
        }


        public Move SaveOrUpdateMove(Move move, String accountOutName, String accountInName, String categoryName)
        {
            VerifyUser();

            BeginTransaction();
            
            try
            {
                var operationType = 
                    move.ID == 0 
                        ? OperationType.Creation 
                        : OperationType.Update;

                move = Parent.BaseMove.SaveOrUpdateMove(move, accountOutName, accountInName, categoryName);

                CommitTransaction();

                Parent.BaseMove.SendEmail(move, operationType);
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
            VerifyUser();

            BeginTransaction();

            try
            {
                var move = SelectMoveById(id);

                VerifyMove(move);


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
            VerifyUser();

            var detail = detailService.SelectById(id);

            if (detail == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidDetail);

            if (detail.Move == null)
                VerifyMove(detail.FutureMove);
            else
                VerifyMove(detail.Move);

            return detail;
        }


    }
}
