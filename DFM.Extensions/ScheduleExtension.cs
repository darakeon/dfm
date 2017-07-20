using System;
using DFM.Entities;

namespace DFM.Extensions
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



        public static Boolean IsFirstMove(this Schedule schedule)
        {
            return schedule.Begin == schedule.Next;
        }




    }
}
