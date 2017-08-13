using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class ScheduleService : GenericMoveService<Schedule, Account>
    {
        internal ScheduleService(IRepository<Schedule> repository) : base(repository) { }



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

        

        



    }
}
