using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
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
            SaveOrUpdate(schedule, complete, validate);
        }

        private static void complete(Schedule schedule)
        {
            if (schedule.ID == 0)
            {
                schedule.Active = true;
                schedule.Begin = schedule.GetLastRunDate();
                schedule.User = schedule.FutureMoveList.First().User();
            }
        }

        private static void validate(Schedule schedule)
        {
            var isCreating = schedule.ID == 0;
            var hasNoFuture = !schedule.FutureMoveList.Any();

            if (isCreating && hasNoFuture)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleWithNoMoves);

            if (!schedule.Boundless && schedule.Times <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleTimesCantBeZero);
        }


        
        internal IList<Schedule> GetScheduleToRun(User user)
        {
            return getRunnableAndDisableOthers(user)
                .ToList();
        }

        private IEnumerable<Schedule> getRunnableAndDisableOthers(User user)
        {
            var scheduleList = 
                List(s => s.User == user
                        && s.Active);

            foreach (var schedule in scheduleList)
            {
                if (!canRun(schedule))
                    deactivate(schedule);
                else if (canRunNow(schedule))
                    yield return schedule;
            }
        }

        private void deactivate(Schedule schedule)
        {
            schedule.Active = false;
            SaveOrUpdate(schedule);
        }

        

        internal DateTime CalculateNextRunDate(Schedule schedule)
        {
            var move = schedule.FutureMoveList.Last();

            return schedule.Frequency.Next(move.Date);
        }



        private static Boolean canRunNow(Schedule schedule)
        {
            return schedule.GetLastRunDate() <= DateTime.Today;
        }

        private static Boolean canRun(Schedule schedule)
        {
            if (!schedule.FutureMoveList.Any())
                return false;

            var doneMoves = schedule.AppliedTimes();

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;

            return schedule.Active &&
                (boundless || !doneAll);
        }



    }
}
