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
                    : schedule.Frequency.Next(move.Date);
        }


        public static void Deactivate(this Schedule schedule)
        {
            schedule.Active = false;
        }


        public static Boolean IsFirstMove(this Schedule schedule)
        {
            return schedule.Begin == schedule.Next;
        }


        public static Boolean CanRun(this Schedule schedule)
        {
            var doneMoves = schedule.appliedTimes();

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;

            return schedule.Active && 
                (boundless || !doneAll);
        }


        public static Boolean CanRunNow(this Schedule schedule)
        {
            return schedule.CanRun() &&
                   schedule.Next <= DateTime.Today;
        }


        private static Int32 appliedTimes(this Schedule schedule)
        {
            return schedule.Frequency
                .AppliedTimes(schedule.Begin, schedule.Next);
        }


    }
}
