using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Services;
using DFM.Entities;

namespace DFM.BusinessLogic.SuperServices
{
    public class RobotService
    {
        private readonly ScheduleService scheduleService;

        internal RobotService(ScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }


        public void SaveOrUpdateSchedule(Schedule schedule)
        {
            scheduleService.SaveOrUpdate(schedule);
        }

        public IList<Schedule> GetScheduleToRun(User user)
        {
            return scheduleService.GetScheduleToRun(user);
        }

        public void SetNextRun(Schedule schedule)
        {
            scheduleService.SetNextRun(schedule);
        }

        public Boolean CanRun(Schedule schedule)
        {
            return scheduleService.CanRun(schedule);
        }

        public Boolean CanRunNow(Schedule schedule)
        {
            return scheduleService.CanRunNow(schedule);
        }


    }
}
