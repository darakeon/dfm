using System;
using System.Linq;
using DFM.Core.Enums;

namespace DFM.Core.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Boolean Contains(this Schedule schedule, Move move)
        {
            return schedule.MoveList.Contains(move);
        }

        public static void AddMove(this Schedule schedule, Move move)
        {
            move.Schedule = schedule;
            schedule.MoveList.Add(move);
        }

        public static void SetNextRun(this Schedule schedule)
        {
            var move = schedule.MoveList.Last();

            schedule.Next =
                move.Date > DateTime.Now 
                    ? move.Date
                    : schedule.Frequency == ScheduleFrequency.Monthly
                        ? move.Date.AddMonths(1)
                        : move.Date.AddYears(1);
        }

        public static void Deactivate(this Schedule schedule)
        {
            schedule.Active = false;
        }


        public static Boolean RunningFirstMove(this Schedule schedule)
        {
            return schedule.Begin == schedule.Next;
        }

    

    }
}
