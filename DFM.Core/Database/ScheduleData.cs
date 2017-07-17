using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;
using DFM.Core.Robots;

namespace DFM.Core.Database
{
    public class ScheduleData : BaseData<Schedule>
    {
        private ScheduleData() { }

        public static void SaveOrUpdate(Schedule schedule)
        {
            //var scheduleTransac = Session.BeginTransaction();
            SaveOrUpdate(schedule, complete, null);
            //scheduleTransac.Commit();
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
            var move = schedule.MoveList.Last();

            schedule.Active = true;
            schedule.Begin = move.Date;

            schedule.SetNextRun();

            var user = (move.Out ?? move.In)
                            .Year.Account.User;

            schedule.User = user;

            //ScheduleRunner.CreateMovesUntilNow(schedule);
        }

    }
}
