using System;
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
        //private readonly RobotService robotService;
        //private User user { get; set; }
        //private Account accountIn { get; set; }
        //private Account accountOut { get; set; }
        //private event Format.GetterForMove formatGetter;


        public ScheduleRunner(User user, Format.GetterForMove formatGetter, RobotService robotService)
        {
            //this.robotService = robotService;
            //this.user = user;
            //this.formatGetter += formatGetter;
        }

        public void Run()
        {
            //var scheduleList = robotService.GetScheduleToRun(user);

            //foreach (var schedule in scheduleList)
            //{
            //    createMovesUntilNow(schedule);
            //}
        }


        //private void createMovesUntilNow(Schedule schedule)
        //{
        //    setAccounts(schedule);

        //    while (robotService.CanRunNow(schedule))
        //    {
        //        var move = getNextMove(schedule);

        //        if (move == null)
        //            return;

        //        ajustSchedule(schedule, move);
                
        //        save(move);
        //    }
        //}


        //private void setAccounts(Schedule schedule)
        //{
        //    var lastMove = schedule.MoveList.LastOrDefault();

        //    if (lastMove == null)
        //        return;

        //    accountIn = lastMove.In;
        //    accountOut = lastMove.Out;
        //}


        //private static Move getNextMove(Schedule schedule)
        //{
        //    var lastMove = schedule.MoveList.LastOrDefault();

        //    if (lastMove == null)
        //        return null;

        //    var newMove = lastMove.ConvertToOtherChild<Move>();

        //    if (!schedule.IsFirstMove())
        //    {
        //        newMove.Date = schedule.Next;
        //    }

        //    return newMove;
        //}


        
        //private void ajustSchedule(Schedule schedule, Move move)
        //{
        //    //if (!schedule.IsFirstMove())
        //    //    schedule.AddMove(move);

        //    robotService.SetNextRun(schedule);
        //}


        //private void save(Move newMove)
        //{
        //    Services.Money.SaveOrUpdateMove(newMove, accountOut, accountIn, formatGetter);
        //}

    }
}
