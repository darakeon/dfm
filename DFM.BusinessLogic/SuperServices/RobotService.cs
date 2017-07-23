using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class RobotService : BaseSuperService
    {
        private readonly ScheduleService scheduleService;
        private readonly FutureMoveService futureMoveService;
        private readonly DetailService detailService;
        
        internal RobotService(ServiceAccess serviceAccess, ScheduleService scheduleService, FutureMoveService futureMoveService, DetailService detailService)
            : base(serviceAccess)
        {
            this.scheduleService = scheduleService;
            this.futureMoveService = futureMoveService;
            this.detailService = detailService;
        }



        #region Scheduler
        public void RunSchedule()
        {
            if (Parent.Current.User == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);

            var scheduleList = scheduleService.GetScheduleToRun(Parent.Current.User);

            foreach (var schedule in scheduleList)
            {
                ajustFutureMovesAndGetFirst(schedule);
            }

            var futureMoves = scheduleList
                .SelectMany(s => s.FutureMoveList
                                .Where(m => m.Date <= DateTime.Now)
                                .ToList()
                            );

            foreach (var futureMove in futureMoves)
            {
                transformFutureInMove(futureMove);
            }
        }



        private void transformFutureInMove(FutureMove futureMove)
        {
            BeginTransaction();

            try
            {
                var schedule = futureMove.Schedule;

                var boundless = schedule.Boundless;
                var isLast = futureMove.ID == schedule.FutureMoveList.Last().ID;

                if (boundless && isLast)
                    addNextFutureMove(futureMove.Schedule);

                var accountOutName = futureMove.Out == null ? null : futureMove.Out.Name;
                var accountInName = futureMove.In == null ? null : futureMove.In.Name;
                var category = futureMove.Category;

                var move = futureMove.CastToKill();

                Parent.BaseMove.SaveOrUpdateMove(move, accountOutName, accountInName, category.Name);

                futureMoveService.Delete(futureMove.ID);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }



        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, String accountOutName, String accountInName, String categoryName, Schedule schedule)
        {
            BeginTransaction();

            try
            {
                placeAccountsInMove(futureMove, accountOutName, accountInName);

                futureMove = saveOrUpdateSchedule(futureMove, categoryName, schedule);
                
                CommitTransaction();

                return futureMove;
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private void placeAccountsInMove(FutureMove futureMove, String accountOutName, String accountInName)
        {
            futureMove.Out = accountOutName == null 
                ? null : Parent.Admin.SelectAccountByName(accountOutName);

            futureMove.In = accountInName == null 
                ? null : Parent.Admin.SelectAccountByName(accountInName);
        }

        private FutureMove saveOrUpdateSchedule(FutureMove futureMove, String categoryName, Schedule schedule)
        {
            if (schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            Parent.BaseMove.SetCategory(futureMove, categoryName);

            if (!schedule.FutureMoveList.Contains(futureMove))
                schedule.FutureMoveList.Add(futureMove);

            futureMove = ajustFutureMovesAndGetFirst(schedule);

            futureMove = saveMove(futureMove);

            return futureMove;
        }

        private FutureMove ajustFutureMovesAndGetFirst(Schedule schedule)
        {
            var firstFMove = schedule.FutureMoveList.First();

            firstFMove.Schedule = schedule;


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
            var nextDate = scheduleService.CalculateNextRunDate(schedule);

            var lastFMove = schedule.FutureMoveList.Last();
            
            var nextFMove = lastFMove.CloneChangingDate(nextDate);

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
