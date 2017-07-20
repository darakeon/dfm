using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class ScheduleService : BaseService<Schedule>
    {
        internal ScheduleService(IRepository<Schedule> repository) : base(repository) { }

        internal void SaveOrUpdate(Schedule schedule)
        {
            SaveOrUpdate(schedule, complete);
        }

        internal IList<Schedule> GetScheduleToRun(User user)
        {
            return getRunnableAndDisableOthers(
                    user.ScheduleList
                        .Where(s => s.Active 
                            && s.Next <= DateTime.Today))
                .ToList();
        }

        private IEnumerable<Schedule> getRunnableAndDisableOthers(IEnumerable<Schedule> scheduleList)
        {
            foreach (var schedule in scheduleList)
            {
                if (CanRun(schedule))
                    yield return schedule;
                else
                    deactivate(schedule);
            }
        }

        private void deactivate(Schedule schedule)
        {
            schedule.Active = false;
            SaveOrUpdate(schedule);
        }

        
        private void complete(Schedule schedule)
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



        internal void SetNextRun(Schedule schedule)
        {
            var move = schedule.MoveList.Last();

            schedule.Next =
                move.Date > DateTime.Now
                    ? move.Date
                    : schedule.Frequency.Next(move.Date);
        }



        internal Boolean CanRun(Schedule schedule)
        {
            var hasMoveToClone = schedule.MoveList.Any();

            var doneMoves = appliedTimes(schedule);

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;

            return schedule.Active && 
                hasMoveToClone &&
                (boundless || !doneAll);
        }


        internal Boolean CanRunNow(Schedule schedule)
        {
            return CanRun(schedule) &&
                   schedule.Next <= DateTime.Today;
        }


        private static Int32 appliedTimes(Schedule schedule)
        {
            return schedule.Frequency
                .AppliedTimes(schedule.Begin, schedule.Next);
        }


        internal void AjustSchedule(Move move)
        {
            if (move.Schedule == null
                || move.Schedule.ID != 0) return;

            if (!move.Schedule.Contains(move))
                move.Schedule.AddMove(move);

            SaveOrUpdate(move.Schedule);
        }



    }
}
