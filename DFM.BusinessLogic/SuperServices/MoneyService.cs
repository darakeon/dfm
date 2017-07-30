using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
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


        public Move GetMoveById(Int32 id)
        {
            VerifyUser();

            var move = moveService.GetById(id);

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
                        : OperationType.Edit;

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
                var move = GetMoveById(id);

                VerifyMove(move);


                Parent.BaseMove.RemoveFromSummaries(move);


                moveService.Delete(id);

                if (move.Schedule != null)
                {
                    move.Schedule.Times--;
                    scheduleService.SaveOrUpdate(move.Schedule);
                }

                CommitTransaction();

                Parent.BaseMove.SendEmail(move, OperationType.Delete);

            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }

        }



        public Detail GetDetailById(Int32 id)
        {
            VerifyUser();

            var detail = detailService.GetById(id);

            if (detail == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidDetail);

            VerifyMove(detail.Move);

            return detail;
        }


    }
}
