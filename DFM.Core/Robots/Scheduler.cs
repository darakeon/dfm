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
                var lastMove = schedule.MoveList.Last();
                var newMove = lastMove.Clone();

                
            }
        }
    }
}
