using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class RobotService : BaseSuperService
    {
        private readonly ScheduleService scheduleService;
        
        internal RobotService(ServiceAccess serviceAccess, ScheduleService scheduleService, DetailService detailService)
            : base(serviceAccess)
        {
            this.scheduleService = scheduleService;
        }



        public void RunSchedule()
        {
            VerifyUser();

            var scheduleList = scheduleService.GetScheduleToRun(Parent.Current.User);

            foreach (var schedule in scheduleList)
            {
                var accountOutName = schedule.Out == null ? null : schedule.Out.Name;
                var accountInName = schedule.In == null ? null : schedule.In.Name;
                var categoryName = schedule.Category.Name;

                addNewMoves(schedule, accountOutName, accountInName, categoryName);
            }
        }

        private void addNewMoves(Schedule schedule, String accountOutName, String accountInName, String categoryName)
        {
            while (schedule.CanRun())
            {
                var newMove = schedule.GetNewMove();

                schedule.LastRun++;

                Parent.BaseMove.SaveOrUpdateMove(newMove, accountOutName, accountInName, categoryName);

                schedule.MoveList.Add(newMove);
            }
        }



        public Schedule SaveOrUpdateSchedule(Schedule schedule, String accountOutName, String accountInName, String categoryName)
        {
            VerifyUser();

            BeginTransaction();

            try
            {
                if (schedule == null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

                linkToEntities(schedule, accountOutName, accountInName, categoryName);

                schedule = scheduleService.SaveOrUpdate(schedule);

                CommitTransaction();

                return schedule;
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private void linkToEntities(Schedule schedule, String accountOutName, String accountInName, String categoryName)
        {
            schedule.Out = accountOutName == null
                ? null : Parent.Admin.GetAccountByName(accountOutName);

            schedule.In = accountInName == null
                ? null : Parent.Admin.GetAccountByName(accountInName);

            schedule.Category = Parent.Admin.GetCategoryByName(categoryName);

            schedule.User = Parent.Current.User;
        }


    }
}
