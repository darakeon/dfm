using System;
using System.Collections.Generic;
using System.Linq;
using Ak.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
    public class RobotService : BaseService
    {
        private readonly ScheduleRepository scheduleRepository;
        private readonly DetailRepository detailRepository;
        
        internal RobotService(ServiceAccess serviceAccess, ScheduleRepository scheduleRepository, DetailRepository detailRepository)
            : base(serviceAccess)
        {
            this.scheduleRepository = scheduleRepository;
            this.detailRepository = detailRepository;
        }



        public Boolean RunSchedule()
        {
            Parent.Safe.VerifyUser();

            var useCategories = Parent.Current.User.Config.UseCategories;

            var emailsSent = runScheduleEqualConfig(useCategories)
                & runScheduleDiffConfig(useCategories);

            Parent.BaseMove.FixSummaries();

            return emailsSent;
        }

        private Boolean runScheduleEqualConfig(Boolean useCategories)
        {
            var scheduleList = scheduleRepository.GetRunnable(Parent.Current.User, useCategories);

            var sameConfigList = scheduleList
                .Where(s => s.HasCategory() == useCategories);

            return runSchedule(sameConfigList);
        }

        private Boolean runScheduleDiffConfig(Boolean useCategories)
        {
            var scheduleList = scheduleRepository.GetRunnable(Parent.Current.User, useCategories);

            var diffConfigList = scheduleList
                .Where(s => s.HasCategory() != useCategories);

            Boolean emailsSent;

            try
            {
                Parent.Admin.UpdateConfig(null, null, null, !useCategories);
                emailsSent = runSchedule(diffConfigList);
            }
            finally
            {
                Parent.Admin.UpdateConfig(null, null, null, useCategories);
            }

            return emailsSent;
        }

        private Boolean runSchedule(IEnumerable<Schedule> scheduleList)
        {
            var emailsSent = true;

            foreach (var schedule in scheduleList)
            {
                var accountOutUrl = schedule.Out == null ? null : schedule.Out.Url;
                var accountInUrl = schedule.In == null ? null : schedule.In.Url;

                var category = schedule.Category;
                var categoryName = category == null ? null : schedule.Category.Name;

                emailsSent &= addNewMoves(schedule, accountOutUrl, accountInUrl, categoryName);
            }

            return emailsSent;
        }

        private Boolean addNewMoves(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
        {
            var emailsSent = true;

            while (schedule.CanRunNow())
            {
                var newMove = schedule.GetNewMove();

                schedule.LastRun++;

                emailsSent &= saveOrUpdateMove(newMove, accountOutUrl, accountInUrl, categoryName);

                schedule.MoveList.Add(newMove);
            }

            return emailsSent;
        }

        private Boolean saveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            ErrorComposedReturn<Move, Boolean> result;

            BeginTransaction();

            try
            {
                result = Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName);
                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }

            return !result.Error;
        }



        public Schedule SaveOrUpdateSchedule(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
        {
            Parent.Safe.VerifyUser();

            BeginTransaction();

            if (schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            var operationType =
                schedule.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            try
            {
                linkToEntities(schedule, accountOutUrl, accountInUrl, categoryName);

                schedule = saveOrUpdate(schedule);

                CommitTransaction();

                return schedule;
            }
            catch
            {
                if (operationType == OperationType.Creation)
                    schedule.ID = 0;

                RollbackTransaction();
                throw;
            }
        }

        private Schedule saveOrUpdate(Schedule schedule)
        {
            if (schedule.ID == 0 || !schedule.IsDetailed())
            {
                scheduleRepository.SaveOrUpdate(schedule);
                detailRepository.SaveDetails(schedule);
            }
            else
            {
                detailRepository.SaveDetails(schedule);
                scheduleRepository.SaveOrUpdate(schedule);
            }

            return schedule;
        }

        private void linkToEntities(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
        {
            schedule.Out = Parent.BaseMove.GetAccountByUrl(accountOutUrl);
            schedule.In = Parent.BaseMove.GetAccountByUrl(accountInUrl);
            schedule.Category = Parent.BaseMove.GetCategoryByName(categoryName);
            schedule.User = Parent.Current.User;
        }


    }
}
