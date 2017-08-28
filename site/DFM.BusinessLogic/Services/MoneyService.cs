using System;
using DK.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Email;
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

            var move = moveRepository.Get(id);

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

            return InTransaction(
				() => Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName),
				() => resetMove(move, operationType)
			);
        }

	    private static void resetMove(Move move, OperationType operationType)
	    {
		    if (operationType == OperationType.Creation)
		    {
			    move.ID = 0;
		    }
	    }


		public ComposedResult<Boolean, EmailStatus> DeleteMove(Int32 id)
        {
            ComposedResult<Boolean, EmailStatus> result;

            Parent.Safe.VerifyUser();

            result = InTransaction(() => deleteMove(id));

            Parent.BaseMove.FixSummaries();

            return result;
        }

        private ComposedResult<Boolean, EmailStatus> deleteMove(Int32 id)
        {
            var move = GetMoveById(id);

            verifyMove(move);

            moveRepository.Delete(id);

            Parent.BaseMove.BreakSummaries(move);

            if (move.Schedule != null)
            {
                updateScheduleDeleted(move.Schedule);
            }

            var emailStatus = moveRepository.SendEmail(move);

            return new ComposedResult<Boolean, EmailStatus>(true, emailStatus);
        }

        private void updateScheduleDeleted(Schedule schedule)
        {
            schedule.Deleted++;

            var useCategories = schedule.User.Config.UseCategories;

            if (schedule.Category == null && useCategories)
            {
                Parent.Admin.UpdateConfigWithinTransaction(null, null, null, false);
            }

            if (schedule.Category != null && !useCategories)
            {
                Parent.Admin.UpdateConfigWithinTransaction(null, null, null, true);
            }

            scheduleRepository.SaveOrUpdate(schedule);

            if (schedule.User.Config.UseCategories != useCategories)
            {
                Parent.Admin.UpdateConfigWithinTransaction(null, null, null, useCategories);
            }
        }


        public Detail GetDetailById(Int32 id)
        {
            Parent.Safe.VerifyUser();

            var detail = detailRepository.Get(id);

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
