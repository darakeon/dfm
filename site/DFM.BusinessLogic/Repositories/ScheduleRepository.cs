using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Repositories
{
    internal class ScheduleRepository : GenericMoveRepository<Schedule>
    {
        internal Schedule SaveOrUpdate(Schedule schedule)
        {
            return SaveOrUpdate(schedule, complete, validate);
        }

        private static void complete(Schedule schedule)
        {
            Complete(schedule);

            if (schedule.ID == 0)
            {
                schedule.Active = true;
            }
        }

        private static void validate(Schedule schedule)
        {
            Validate(schedule);

            if (schedule.Active)
                TestCategory(schedule);

            if (!schedule.Boundless && schedule.Times <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleTimesCantBeZero);
        }


        
        internal IList<Schedule> GetRunnable(User user, Boolean hasCategory)
        {
            return getRunnableAndDisableOthers(user, hasCategory).ToList();
        }

        private IEnumerable<Schedule> getRunnableAndDisableOthers(User user, Boolean hasCategory)
        {
            var scheduleList = 
                List(s => s.User == user && s.Active)
                    .Where(s => s.HasCategory() == hasCategory);

            foreach (var schedule in scheduleList)
            {
                if (!schedule.CanRun())
                    disable(schedule);
                else if (schedule.CanRunNow())
                    yield return schedule;
            }
        }

        private void disable(Schedule schedule)
        {
            schedule.Active = false;
            SaveOrUpdate(schedule);
        }



        internal void Disable(int id, User user)
        {
            var schedule = getById(id, user);

            if (schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidSchedule);

            if (!schedule.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledSchedule);

            schedule.Active = false;
            SaveOrUpdate(schedule);
        }

        private Schedule getById(Int32 id, User user)
        {
            return SingleOrDefault(
                    a => a.ID == id
                         && a.User.ID == user.ID
                );
        }



    }
}
