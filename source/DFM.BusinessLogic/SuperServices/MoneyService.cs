using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.SuperServices
{
    public class MoneyService : BaseService
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

            move = saveOrUpdate(move, accountOutName, accountInName, categoryName);

            Parent.BaseMove.FixSummaries();

            return move;
        }

        private Move saveOrUpdate(Move move, string accountOutName, string accountInName, string categoryName)
        {
            BeginTransaction();

            var operationType =
                move.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            try
            {
                move = Parent.BaseMove.SaveOrUpdateMove(move, accountOutName, accountInName, categoryName);

                Parent.BaseMove.SendEmail(move, operationType);

                CommitTransaction();
            }
            catch
            {
                if (operationType == OperationType.Creation)
                    move.ID = 0;

                RollbackTransaction();
                throw;
            }

            return move;
        }


        public void DeleteMove(Int32 id)
        {
            VerifyUser();

            deleteMove(id);
        }

        private void deleteMove(int id)
        {
            BeginTransaction();

            try
            {
                var move = GetMoveById(id);

                VerifyMove(move);

                moveService.Delete(id);

                Parent.BaseMove.BreakSummaries(move);

                if (move.Schedule != null)
                {
                    move.Schedule.Deleted++;

                    scheduleService.SaveOrUpdate(move.Schedule);
                }

                Parent.BaseMove.SendEmail(move, OperationType.Delete);

                CommitTransaction();

            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }

            Parent.BaseMove.FixSummaries();
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
