using System.Linq;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;
using DFM.Core.Exceptions;

namespace DFM.Core.Robots
{
    internal class ScheduleRunner
    {
        private static Account accountIn { get; set; }
        private static Account accountOut { get; set; }
        
        public static void Run(User user)
        {
            var scheduleList = ScheduleData.GetScheduleToRun(user);

            foreach (var schedule in scheduleList)
            {
                CreateMovesUntilNow(schedule);
            }
        }


        internal static void CreateMovesUntilNow(Schedule schedule)
        {
            while (schedule.CanRunNow())
            {
                var move = getNextMove(schedule);

                if (move == null)
                    return;

                ajustSchedule(schedule, move);
                
                save(move);
            }
        }


        private static Move getNextMove(Schedule schedule)
        {
            var lastMove = schedule.MoveList.LastOrDefault();

            if (lastMove == null)
            {
                schedule.Deactivate();
                ScheduleData.SaveOrUpdate(schedule);
                return null;
            }
            

            accountIn = getAccount(lastMove.In);
            accountOut = getAccount(lastMove.Out);

            if (schedule.IsFirstMove())
                return lastMove;


            var newMove = lastMove.Clone();

            newMove.In = null;
            newMove.Out = null;
            newMove.Date = schedule.Next;

            return newMove;
        }


        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
        }

        
        private static void ajustSchedule(Schedule schedule, Move move)
        {
            if (!schedule.IsFirstMove())
                schedule.AddMove(move);

            if (schedule.CanRun())
                schedule.SetNextRun();
            else
                schedule.Deactivate();
        }


        private static void save(Move newMove)
        {
            MoveData.SaveOrUpdate(newMove, accountOut, accountIn);
        }

    }
}
