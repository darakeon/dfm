using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
    public class MoneyService : BaseService
    {
        private readonly MoveRepository moveService;
        private readonly DetailRepository detailService;
        private readonly ScheduleRepository scheduleService;

        internal MoneyService(ServiceAccess serviceAccess, MoveRepository moveService, DetailRepository detailService, ScheduleRepository scheduleService)
            : base(serviceAccess)
        {
            this.moveService = moveService;
            this.detailService = detailService;
            this.scheduleService = scheduleService;
        }


        public Move GetMoveById(Int32 id)
        {
            VerifyUser();

            var move = moveService.GetById(id);

            VerifyMove(move);

            return move;
        }


        public Move SaveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            VerifyUser();

            move = saveOrUpdate(move, accountOutUrl, accountInUrl, categoryName);

            Parent.BaseMove.FixSummaries();

            return move;
        }

        private Move saveOrUpdate(Move move, String accountOutUrl, String accountInUrl, string categoryName)
        {
            BeginTransaction();

            var operationType =
                move.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            try
            {
                move = Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName);

                if (Parent.Current.User.SendMoveEmail)
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
