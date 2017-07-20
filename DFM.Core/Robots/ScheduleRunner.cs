using System;
using System.Linq;
using DFM.Core.Database;
using DFM.Core.Email;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Robots
{
    internal class ScheduleRunner
    {
        private User user { get; set; }
        private Account accountIn { get; set; }
        private Account accountOut { get; set; }
        private event Format.GetterForMove formatGetter;


        public ScheduleRunner(User user, Format.GetterForMove formatGetter)
        {
            this.user = user;
            this.formatGetter += formatGetter;
        }

        public void Run()
        {
            var scheduleList = ScheduleData.GetScheduleToRun(user);

            foreach (var schedule in scheduleList)
            {
                createMovesUntilNow(schedule);
            }
        }


        private void createMovesUntilNow(Schedule schedule)
        {
            setAccounts(schedule);

            while (schedule.CanRunNow())
            {
                var move = getNextMove(schedule);

                if (move == null)
                    return;

                ajustSchedule(schedule, move);
                
                save(move);
            }
        }


        private void setAccounts(Schedule schedule)
        {
            var lastMove = schedule.MoveList.LastOrDefault();

            if (lastMove == null)
                return;

            accountIn = lastMove.AccountIn();
            accountOut = lastMove.AccountOut();
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


            if (schedule.IsFirstMove())
                return lastMove;


            var newMove = lastMove.Clone();

            newMove.In = null;
            newMove.Out = null;
            newMove.Date = schedule.Next;

            return newMove;
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


        private void save(Move newMove)
        {
            MoveData.SaveOrUpdate(newMove, accountOut, accountIn, formatGetter);
        }

    }
}
