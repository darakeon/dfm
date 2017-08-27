using System;
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



        public void RunSchedule()
        {
            VerifyUser();

            var scheduleList = scheduleRepository.GetScheduleToRun(Parent.Current.User);

            foreach (var schedule in scheduleList)
            {
                var accountOutUrl = schedule.Out == null ? null : schedule.Out.Url;
                var accountInUrl = schedule.In == null ? null : schedule.In.Url;
                var categoryName = schedule.Category.Name;

                addNewMoves(schedule, accountOutUrl, accountInUrl, categoryName);
            }

            Parent.BaseMove.FixSummaries();
        }

        private void addNewMoves(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
        {
            while (schedule.CanRunNow())
            {
                var newMove = schedule.GetNewMove();

                schedule.LastRun++;

                saveOrUpdateMove(newMove, accountOutUrl, accountInUrl, categoryName);

                schedule.MoveList.Add(newMove);
            }
        }

        private void saveOrUpdateMove(Move move, String accountOutUrl, String accountInUrl, String categoryName)
        {
            BeginTransaction();

            try
            {
                Parent.BaseMove.SaveOrUpdateMove(move, accountOutUrl, accountInUrl, categoryName);
                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }



        public Schedule SaveOrUpdateSchedule(Schedule schedule, String accountOutUrl, String accountInUrl, String categoryName)
        {
            VerifyUser();

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
