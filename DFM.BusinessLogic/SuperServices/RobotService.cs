using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.SuperServices
{
    public class RobotService : BaseSuperService
    {
        private readonly ScheduleService scheduleService;
        private readonly DetailService detailService;
        
        internal RobotService(ServiceAccess serviceAccess, ScheduleService scheduleService, DetailService detailService)
            : base(serviceAccess)
        {
            this.scheduleService = scheduleService;
            this.detailService = detailService;
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
            while (schedule.CanRunNow())
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

            if (schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            var operationType =
                schedule.ID == 0
                    ? OperationType.Creation
                    : OperationType.Edit;

            try
            {
                linkToEntities(schedule, accountOutName, accountInName, categoryName);

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
            scheduleService.SaveOrUpdate(schedule);

            detailService.SaveDetails(schedule);

            return schedule;
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
