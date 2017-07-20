using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    public class ScheduleService : BaseService<Schedule>
    {
        internal ScheduleService(DataAccess father, IRepository repository) : base(father, repository) { }

        public void SaveOrUpdate(Schedule schedule)
        {
            SaveOrUpdate(schedule, complete);
        }

        public static IList<Schedule> GetScheduleToRun(User user)
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
                SetNextRun(schedule);
        }



        public static void SetNextRun(Schedule schedule)
        {
            var move = schedule.MoveList.Last();

            schedule.Next =
                move.Date > DateTime.Now
                    ? move.Date
                    : schedule.Frequency.Next(move.Date);
        }



        public static Boolean CanRun(Schedule schedule)
        {
            var doneMoves = appliedTimes(schedule);

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;

            return schedule.Active &&
                (boundless || !doneAll);
        }


        public static Boolean CanRunNow(Schedule schedule)
        {
            return CanRun(schedule) &&
                   schedule.Next <= DateTime.Today;
        }


        private static Int32 appliedTimes(Schedule schedule)
        {
            return schedule.Frequency
                .AppliedTimes(schedule.Begin, schedule.Next);
        }


    }
}
