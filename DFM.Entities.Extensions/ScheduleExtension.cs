using System;
using System.Linq;

namespace DFM.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Boolean Contains(this Schedule schedule, FutureMove futureMove)
        {
            return schedule.FutureMoveList.Contains(futureMove);
        }


        public static void AddMove(this Schedule schedule, FutureMove futureMove)
        {
            futureMove.Schedule = schedule;
            schedule.FutureMoveList.Add(futureMove);
        }

        public static DateTime GetNextRunDate(this Schedule schedule)
        {
            return schedule.FutureMoveList
                .Min(m => m.Date);
        }

        public static DateTime GetLastRunDate(this Schedule schedule)
        {
            return schedule.FutureMoveList
                .Min(m => m.Date);
        }

        public static User GetUser(this Schedule schedule)
        {
            return schedule.FutureMoveList.FirstOrDefault().User();
        }


        public static Boolean CanRunNow(this Schedule schedule)
        {
            return schedule.GetNextRunDate() <= DateTime.Today;
        }


        public static DateTime LastDate(this Schedule schedule)
        {
            return schedule.FutureMoveList.Max(f => f.Date);
        }



    }
}
