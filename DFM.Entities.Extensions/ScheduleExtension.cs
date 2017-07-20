using System;

namespace DFM.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Boolean Contains(this Schedule schedule, FutureMove futureMove)
        {
            return schedule.MoveList.Contains(futureMove);
        }


        public static void AddMove(this Schedule schedule, FutureMove futureMove)
        {
            futureMove.Schedule = schedule;
            schedule.MoveList.Add(futureMove);
        }



        public static Boolean IsFirstMove(this Schedule schedule)
        {
            return schedule.Begin == schedule.Next;
        }




    }
}
