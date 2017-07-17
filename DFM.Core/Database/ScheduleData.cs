using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    public class ScheduleData : BaseData<Schedule>
    {
        private ScheduleData() { }

        public static void SaveOrUpdate(Schedule schedule)
        {
            SaveOrUpdate(schedule, complete, null);
        }

        internal static IList<Schedule> GetScheduleToRun(User user)
        {
            return user.ScheduleList
                .Where(
                    s => s.Active 
                        && s.Next <= DateTime.Today)
                .ToList();
        }
        
        private static void complete(Schedule schedule)
        {
            if (schedule.ID == 0)
            {
                var move = schedule.MoveList.First();

                schedule.Active = true;
                schedule.Begin = move.Date;

                var user = (move.Out ?? move.In)
                    .Year.Account.User;

                schedule.User = user;
            }

            if (schedule.Active)
                schedule.SetNextRun();
        }

    }
}
