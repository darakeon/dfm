using System;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public partial class Schedule
    {
        public virtual Move GetNewMove()
        {
            var dateTime = LastDateRun();

            var move =
                new Move
                    {
                        Date = dateTime,
                        Description = Description,
                        Nature = Nature,
                        Schedule = this,
                    };

            foreach (var detail in DetailList)
            {
                move.AddDetail(detail.Clone());
            }

            return move;
        }


        public virtual DateTime LastDateRun()
        {
            switch (Frequency)
            {
                case ScheduleFrequency.Monthly:
                    return Date.AddMonths(LastRun);
                case ScheduleFrequency.Yearly:
                    return Date.AddYears(LastRun);
                case ScheduleFrequency.Daily:
                    return Date.AddDays(LastRun);
                default:
                    throw new ArgumentException("schedule");
            }
        }



        public virtual Boolean CanRun()
        {
            return canRun(false);
        }

        public virtual Boolean CanRunNow()
        {
            return canRun(true);
        }

        private Boolean canRun(Boolean tryNow)
        {
            if (!Active)
                return false;

            var lastDate = LastDateRun();

            if (tryNow && lastDate >= User.Now())
                return false;

            if (Boundless)
                return true;

            return LastRun < Times;
        }


    }
}
