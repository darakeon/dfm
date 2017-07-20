using System;
using System.Linq;
using DFM.BusinessLogic.SuperServices;
using DFM.Entities;
using DFM.Repositories;

namespace DFM.Robot
{
    internal class ScheduleRunner
    {
        private readonly RobotService robotService;
        private readonly User user;


        public ScheduleRunner(User user, RobotService robotService)
        {
            this.robotService = robotService;
            this.user = user;
        }

        public void Run()
        {
            var scheduleList = robotService.GetScheduleToRun(user);

            //TODO: colocar isto no serviço
            var futureMoves = scheduleList
                .Select(s => s.FutureMoveList
                                .Where(m => m.Date <= DateTime.Now).ToList())
                .SelectMany(moves => moves);

            foreach (var futureMove in futureMoves)
            {
                Services.Robot.TransformFutureInMove(futureMove);
            }

        }
    }
}
