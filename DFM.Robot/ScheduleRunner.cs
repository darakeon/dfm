using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.SuperServices;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Repositories;

namespace DFM.Robot
{
    internal class ScheduleRunner
    {
        private readonly RobotService robotService;
        private User user { get; set; }
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
                    .Where(m => m.Date <= DateTime.Now);

                transformToMoves(moves);
            }
        }


        private void transformToMoves(IEnumerable<FutureMove> futureMoveList)
        {
            foreach (var futureMove in futureMoveList)
            {
                var move = futureMove.Cast();

                Services.Money.SaveOrUpdateMove(move, futureMove.Out, futureMove.In, formatGetter);
                Services.Money.DeleteMove(futureMove);
            }
        }


    }
}
