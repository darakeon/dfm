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
            //var scheduleTransac = Session.BeginTransaction();
            SaveOrUpdate(schedule, complete, null);
            //scheduleTransac.Commit();
        }

        public static IList<Schedule> GetScheduleToRun(User user)
        {
            var criteria = CreateSimpleCriteria(
                s => s.Active 
                    && s.Next <= DateTime.Today 
                    && s.User.ID == user.ID);

            return criteria.List<Schedule>();
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
        }
    }
}
