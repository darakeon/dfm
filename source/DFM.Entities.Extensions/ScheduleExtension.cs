using System;
using System.Linq;
using DFM.Entities.Enums;

namespace DFM.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Move GetNewMove(this Schedule schedule)
        {
            var dateTime = schedule.LastDateRun();

            var move =
                new Move
                    {
                        Date = dateTime,
                        Description = schedule.Description,
                        Nature = schedule.Nature,
                        Schedule = schedule,
                    };

            foreach (var detail in schedule.DetailList)
            {
                move.AddDetail(detail.Clone());
            }

            return move;
        }


        public static DateTime LastDateRun(this Schedule schedule)
        {
            switch (schedule.Frequency)
            {
                case ScheduleFrequency.Monthly:
                    return schedule.Date.AddMonths(schedule.LastRun);
                case ScheduleFrequency.Yearly:
                    return schedule.Date.AddYears(schedule.LastRun);
                case ScheduleFrequency.Daily:
                    return schedule.Date.AddDays(schedule.LastRun);
                default:
                    throw new ArgumentException("schedule");
            }
        }



        public static Boolean CanRun(this Schedule schedule)
        {
            return canRun(schedule, false);
        }

        public static Boolean CanRunNow(this Schedule schedule)
        {
            return canRun(schedule, true);
        }

        private static Boolean canRun(this Schedule schedule, Boolean now)
        {
            if (!schedule.Active)
                return false;

            var lastDate = schedule.LastDateRun();

            if (now && lastDate >= DateTime.Now)
                return false;

            if (schedule.Boundless)
                return true;

            return schedule.LastRun < schedule.Times;
        }


    }
}
