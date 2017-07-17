using System;
using System.Linq;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Robots
{
    public class ScheduleRunner
    {
        private static Account accountIn { get; set; }
        private static Account accountOut { get; set; }
        
        /*
         * Verificar se existe um que esteja ativo e que a data de next seja menor que hoje
         * Ao finalizar o agendamento, colocar como inativo
         */



        public static void Run(User user)
        {
            var scheduleList = ScheduleData.GetScheduleToRun(user);

            foreach (var schedule in scheduleList)
            {
                while (schedule.Active &&
                    schedule.Next <= DateTime.Today)
                {
                    var move = getNextMove(schedule);
                    ajustSchedule(schedule, move);
                    
                    save(move);
                }
            }

        }

        private static Move getNextMove(Schedule schedule)
        {
            var lastMove = schedule.MoveList.Last();

            if (schedule.RunningFirstMove())
                return lastMove;


            var newMove = lastMove.Clone();

            newMove.In = null;
            newMove.Out = null;
            newMove.Date = schedule.Next;

            accountIn = getAccount(lastMove.In);
            accountOut = getAccount(lastMove.Out);

            return newMove;
        }

        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
        }


        
        private static void ajustSchedule(Schedule schedule, Move move)
        {
            if (!schedule.RunningFirstMove())
                schedule.AddMove(move);


            var doneMoves = schedule.MoveList.Count;

            var doneAll = doneMoves >= schedule.Times;
            var boundless = schedule.Boundless;


            if (!boundless && doneAll)
                schedule.Deactivate();
            else
                schedule.SetNextRun();
        }




        private static void save(Move newMove)
        {
            MoveData.SaveOrUpdate(newMove, accountOut, accountIn);
        }

    }
}
