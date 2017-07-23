using System;
using System.Linq;
using DFM.Entities.Bases;

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

        public static DateTime GetLastRunDate(this Schedule schedule)
        {
            return schedule.FutureMoveList
                .Min(m => m.Date);
        }

        public static User GetUser(this Schedule schedule)
        {
            return schedule.FutureMoveList.FirstOrDefault().User();
        }


        public static DateTime LastDate(this Schedule schedule)
        {
            return schedule.FutureMoveList.Max(f => f.Date);
        }



        public static Int32 TotalMoves(this Schedule schedule)
        {
            return schedule.MoveList.Count
                   + schedule.FutureMoveList.Count;
        }

        public static Int32 ExecutedMoves(this Schedule schedule, BaseMove baseMove)
        {
            return baseMove.GetType() == typeof (Move)
                    
                    ? schedule.MoveList
                        .Count(m => m.Date <= baseMove.Date)
                    
                    : schedule.MoveList.Count
                        + schedule.FutureMoveList
                            .Count(m => m.Date <= baseMove.Date);
        }
    }
}
