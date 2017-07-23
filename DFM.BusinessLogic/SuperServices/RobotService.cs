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
        public void RunSchedule(User user)
        {
            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);

            var scheduleList = scheduleService.GetScheduleToRun(user);

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
                transformFutureInMove(futureMove, user);
            }
        }



        private void transformFutureInMove(FutureMove futureMove, User user)
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

                Parent.BaseMove.SaveOrUpdateMove(move, user, accountOutName, accountInName, category);

                futureMoveService.Delete(futureMove.ID);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }



        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, User user, String accountOutName, String accountInName, Category category, Schedule schedule)
        {
            BeginTransaction();

            try
            {
                placeAccountsInMove(futureMove, user, accountOutName, accountInName);

                futureMove = saveOrUpdateSchedule(futureMove, category, schedule);
                
                CommitTransaction();

                return futureMove;
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private void placeAccountsInMove(FutureMove futureMove, User user, String accountOutName, String accountInName)
        {
            futureMove.Out = accountOutName == null 
                ? null : Parent.Admin.SelectAccountByName(accountOutName, user);

            futureMove.In = accountInName == null 
                ? null : Parent.Admin.SelectAccountByName(accountInName, user);
        }

        private FutureMove saveOrUpdateSchedule(FutureMove futureMove, Category category, Schedule schedule)
        {
            if (schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            Parent.BaseMove.SetCategory(futureMove, category);

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
