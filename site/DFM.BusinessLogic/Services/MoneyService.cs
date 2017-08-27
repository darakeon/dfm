using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
    public class MoneyService : BaseService
    {
        private readonly MoveRepository moveRepository;
        private readonly DetailRepository detailRepository;
        private readonly ScheduleRepository scheduleRepository;

        internal MoneyService(ServiceAccess serviceAccess, MoveRepository moveRepository, DetailRepository detailRepository, ScheduleRepository scheduleRepository)
            : base(serviceAccess)
        {
            this.moveRepository = moveRepository;
            this.detailRepository = detailRepository;
            this.scheduleRepository = scheduleRepository;
        }


        public Move GetMoveById(Int32 id)
        {
            Parent.Safe.VerifyUser();

            var move = moveRepository.GetById(id);

            verifyMove(move);

            return move;
        }


        public Move SaveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            Parent.Safe.VerifyUser();

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

                if (Parent.Current.User.Config.SendMoveEmail)
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
            Parent.Safe.VerifyUser();

            deleteMove(id);
        }

        private void deleteMove(int id)
        {
            BeginTransaction();

            try
            {
                var move = GetMoveById(id);

                verifyMove(move);

                moveRepository.Delete(id);

                Parent.BaseMove.BreakSummaries(move);

                if (move.Schedule != null)
                {
                    move.Schedule.Deleted++;

                    scheduleRepository.SaveOrUpdate(move.Schedule);
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
            Parent.Safe.VerifyUser();

            var detail = detailRepository.GetById(id);

            if (detail == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidDetail);

            verifyMove(detail.Move);

            return detail;
        }


        // ReSharper disable once UnusedParameter.Local
        private void verifyMove(Move move)
        {
            if (move == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMove);

            if (move.User.Email != Parent.Current.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }


    }
}
