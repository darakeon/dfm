using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Enums;

namespace DFM.Core.Database
{
    public class ScheduleData : BaseData<Schedule>
    {
        private ScheduleData() { }

        public static Schedule SaveOrUpdate(Schedule schedule)
        {
            return SaveOrUpdate(schedule, null, null);
        }

        public static IList<Schedule> GetScheduleToRun(User user)
        {
            var criteria = CreateSimpleCriteria(
                s => s.Active 
                    && s.Next <= DateTime.Today 
                    && s.User.ID == user.ID);

            return criteria.List<Schedule>();
        }
        
        internal static void Initialize(Schedule schedule)
        {
            var move = schedule.MoveList.Last();

            schedule.Active = true;

            schedule.Begin = move.Date;

            schedule.Next =
                schedule.Frequency == ScheduleFrequency.Monthly
                    ? move.Date.AddMonths(1)
                    : move.Date.AddYears(1);

            var user = (move.In ?? move.Out)
                .Year.Account.User;

            schedule.User = user;
        }
    }
}
