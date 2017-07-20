using System;

namespace DFM.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Boolean Contains(this Schedule schedule, FutureMove move)
        {
            return schedule.MoveList.Contains(move);
        }


        public static void AddMove(this Schedule schedule, FutureMove move)
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
