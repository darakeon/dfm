using System;
using Ak.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Email.Exceptions;
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


        public ComposedResult<Move, EmailStatus> SaveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            Parent.Safe.VerifyUser();

            var result = saveOrUpdate(move, accountOutUrl, accountInUrl, categoryName);

            Parent.BaseMove.FixSummaries();

            return result;
        }

        private ComposedResult<Move, EmailStatus> saveOrUpdate(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            var operationType =
                move.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            ComposedResult<Move, EmailStatus> result;

            BeginTransaction();

            try
            {
                result = Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName);

                CommitTransaction();
            }
            catch
            {
                if (operationType == OperationType.Creation)
                    move.ID = 0;

                RollbackTransaction();
                throw;
            }

            return result;
        }


        public ComposedResult<Boolean, EmailStatus> DeleteMove(Int32 id)
        {
            ComposedResult<Boolean, EmailStatus> result;

            Parent.Safe.VerifyUser();

            BeginTransaction();

            try
            {
                result = deleteMove(id);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }

            Parent.BaseMove.FixSummaries();

            return result;
        }

        private ComposedResult<Boolean, EmailStatus> deleteMove(int id)
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

            var emailStatus = Parent.BaseMove.SendEmail(move, OperationType.Delete);
            return new ComposedResult<Boolean, EmailStatus>(true, emailStatus);
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
