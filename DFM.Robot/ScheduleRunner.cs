using System;
using System.Linq;
using DFM.BusinessLogic.SuperServices;
using DFM.Email;
using DFM.Entities;
using DFM.Repositories;

namespace DFM.Robot
{
    internal class ScheduleRunner
    {
        private readonly RobotService robotService;
        private readonly User user;
        private event Format.GetterForMove formatGetter;


        public ScheduleRunner(User user, Format.GetterForMove formatGetter, RobotService robotService)
        {
            this.robotService = robotService;
            this.user = user;
            this.formatGetter += formatGetter;
        }

        public void Run()
        {
            var scheduleList = robotService.GetScheduleToRun(user);

            foreach (var schedule in scheduleList)
            {
                var moves = schedule.FutureMoveList
                    .Where(m => m.Date <= DateTime.Now)
                    .ToList();

                foreach (var futureMove in moves)
                {
                    Services.Robot
                        .TransformFutureInMove(futureMove, formatGetter);
                }
            }
        }

    }
}
