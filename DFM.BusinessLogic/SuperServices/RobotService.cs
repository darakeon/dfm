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


    }
}
