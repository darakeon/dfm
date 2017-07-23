using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class RobotService
    {
        private readonly MoneyService moneyService;

        private readonly ScheduleService scheduleService;
        private readonly FutureMoveService futureMoveService;
        private readonly DetailService detailService;
        private readonly CategoryService categoryService;
        private readonly AccountService accountService;
        private readonly UserService userService;
        
        internal RobotService(MoneyService moneyService, ScheduleService scheduleService, FutureMoveService futureMoveService, DetailService detailService, CategoryService categoryService, AccountService accountService, UserService userService)
        {
            this.moneyService = moneyService;

            this.scheduleService = scheduleService;
            this.futureMoveService = futureMoveService;
            this.detailService = detailService;
            this.categoryService = categoryService;
            this.accountService = accountService;
            this.userService = userService;
        }



        #region Scheduler
        public void RunSchedule(User user)
        {
            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);

            var scheduleList = scheduleService.GetScheduleToRun(user);

            var futureMoves = scheduleList
                .SelectMany(s => s.FutureMoveList
                                .Where(m => m.Date <= DateTime.Now).ToList());

            foreach (var futureMove in futureMoves)
            {
                transformFutureInMove(futureMove);
            }
        }

        private void transformFutureInMove(FutureMove futureMove)
        {
            futureMoveService.BeginTransaction();

            try
            {
                var schedule = futureMove.Schedule;
                var boundless = schedule.Boundless;
                var isLast = futureMove.ID == schedule.FutureMoveList.Last().ID;

                if (boundless && isLast)
                    addNextFutureMove(futureMove.Schedule);

                var accountOut = futureMove.Out;
                var accountIn = futureMove.In;

                var move = futureMove.CastToKill();

                moneyService.SaveOrUpdateMoveWithOpenTransaction(move, accountOut, accountIn);
                futureMoveService.Delete(futureMove.ID);

                futureMoveService.CommitTransaction();
            }
            catch
            {
                futureMoveService.RollbackTransaction();
                throw;
            }
        }



        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, Account accountOut, Account accountIn)
        {
            futureMove.Out = accountOut == null 
                ? null 
                : accountService.SelectByName(accountOut.Name, accountOut.User);

            futureMove.In = accountIn == null 
                ? null 
                : accountService.SelectByName(accountIn.Name, accountIn.User);

            futureMoveService.BeginTransaction();

            try
            {
                futureMove = saveOrUpdateSchedule(futureMove);
                
                futureMoveService.CommitTransaction();

                return futureMove;
            }
            catch
            {
                futureMoveService.RollbackTransaction();
                throw;
            }
        }

        private FutureMove saveOrUpdateSchedule(FutureMove futureMove)
        {
            if (futureMove.Schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            categoryService.SetCategory(futureMove);

            if (!futureMove.Schedule.FutureMoveList.Contains(futureMove))
                futureMove.Schedule.FutureMoveList.Add(futureMove);

            futureMove = ajustFutureMovesAndGetFirst(futureMove.Schedule);

            futureMove = saveMove(futureMove);

            return futureMove;
        }

        private FutureMove ajustFutureMovesAndGetFirst(Schedule schedule)
        {
            var firstFMove = schedule.FutureMoveList.First();

            if (schedule.Boundless)
            {
                while(schedule.LastDate() < DateTime.Today)
                {
                    addNextFutureMove(schedule);
                }
            }
            else
            {
                var addedFMoveCount = schedule.FutureMoveList.Count;

                for (var fm = addedFMoveCount; fm < schedule.Times; fm++)
                {
                    addNextFutureMove(schedule);
                }
            }


            scheduleService.SaveOrUpdate(schedule);


            return firstFMove;
        }



        private void addNextFutureMove(Schedule schedule)
        {
            var nextDate = scheduleService.GetNextRunDate(schedule);

            var nextFMove = schedule.FutureMoveList
                                .Last()
                                .CloneChangingDate(nextDate);

            schedule.FutureMoveList.Add(nextFMove);

            saveMove(nextFMove);
        }



        private FutureMove saveMove(FutureMove futureMove)
        {
            futureMove = futureMoveService.SaveOrUpdate(futureMove);

            detailService.SaveDetails(futureMove);

            return futureMove;
        }

        #endregion


    }
}
