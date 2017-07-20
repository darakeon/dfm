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
        private readonly ScheduleService scheduleService;
        private readonly FutureMoveService futureMoveService;
        private readonly DetailService detailService;
        private readonly CategoryService categoryService;
        
        private readonly MoneyService moneyService;

        internal RobotService(MoneyService moneyService, ScheduleService scheduleService, FutureMoveService futureMoveService, DetailService detailService, CategoryService categoryService)
        {
            this.scheduleService = scheduleService;
            this.futureMoveService = futureMoveService;
            this.detailService = detailService;
            this.categoryService = categoryService;

            this.moneyService = moneyService;
        }



        #region Scheduler
        public void RunSchedule(User user)
        {
            var scheduleList = scheduleService.GetScheduleToRun(user);

            var futureMoves = scheduleList
                .Select(s => s.FutureMoveList
                                .Where(m => m.Date <= DateTime.Now).ToList())
                .SelectMany(moves => moves);

            foreach (var futureMove in futureMoves)
            {
                transformFutureInMove(futureMove);
            }
        }

        private void transformFutureInMove(FutureMove futureMove)
        {
            var transaction = futureMoveService.BeginTransaction();

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
                futureMoveService.Delete(futureMove);

                futureMoveService.CommitTransaction(transaction);
            }
            catch
            {
                futureMoveService.RollbackTransaction(transaction);
                throw;
            }
        }



        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, Account accountOut, Account accountIn)
        {
            futureMove.Out = accountOut;
            futureMove.In = accountIn;

            var transaction = futureMoveService.BeginTransaction();

            try
            {
                futureMove = saveOrUpdateSchedule(futureMove);
                
                futureMoveService.CommitTransaction(transaction);

                return futureMove;
            }
            catch
            {
                futureMoveService.RollbackTransaction(transaction);
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

            futureMoveService.SaveOrUpdate(nextFMove);

            detailService.SaveDetails(nextFMove);
        }

        #endregion


    }
}
