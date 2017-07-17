using System;
using System.Linq;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Robots
{
    public class ScheduleRunner
    {
        /*
         * Verificar se existe um que esteja ativo e que a data de next seja menor que hoje
         * Ao finalizar o agendamento, colocar como inativo
         */
        public static void Run(User user)
        {
            var scheduleList = ScheduleData.GetScheduleToRun(user);

            foreach (var schedule in scheduleList)
            {
                var newMove = createMove(schedule);
                ajustSchedule(schedule, newMove);
            }
        }



        private static Move createMove(Schedule schedule)
        {
            var lastMove = schedule.MoveList.Last();
            var newMove = lastMove.Clone();

            newMove.In = null;
            newMove.Out = null;
            newMove.Date = schedule.Next;

            var accountIn = getAccount(lastMove.In);
            var accountOut = getAccount(lastMove.Out);

            MoveData.SaveOrUpdate(newMove, accountOut, accountIn);

            return newMove;
        }

        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
        }


        
        private static void ajustSchedule(Schedule schedule, Move move)
        {
            schedule.AddMove(move);

            var doneMoves = schedule.MoveList.Count;

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;


            if (!boundless && doneAll)
                schedule.Deactivate();
            else
                schedule.SetNextRun();


            ScheduleData.SaveOrUpdate(schedule);
        }
    }
}
