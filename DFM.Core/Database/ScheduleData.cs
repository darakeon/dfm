using DFM.Core.Database.Base;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class ScheduleData : BaseData<Schedule>
    {
        private ScheduleData() { }

        public static Schedule SaveOrUpdate(Schedule schedule)
        {
            return SaveOrUpdate(schedule, null, null);
        }

    }
}
