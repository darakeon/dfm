using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Email;
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
        public void SaveOrUpdateSchedule(Schedule schedule)
        {
            scheduleService.SaveOrUpdate(schedule);
        }

        public IList<Schedule> GetScheduleToRun(User user)
        {
            return scheduleService.GetScheduleToRun(user);
        }


        public void DeleteFutureMove(FutureMove move)
        {
            futureMoveService.Delete(move);
        }
        

        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, Account accountOut, Account accountIn)
        {
            if (futureMove.Schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            var transaction = futureMoveService.BeginTransaction();

            try
            {
                categoryService.SetCategory(futureMove);

                if (!futureMove.Schedule.FutureMoveList.Contains(futureMove))
                    futureMove.Schedule.FutureMoveList.Add(futureMove);

                futureMove = ajustFutureMovesAndGetFirst(futureMove.Schedule, accountOut, accountIn);


                futureMoveService.CommitTransaction(transaction);
            }
            catch (Exception)
            {
                futureMoveService.RollbackTransaction(transaction);
                throw;
            }

            return futureMove;
        }

        private FutureMove ajustFutureMovesAndGetFirst(Schedule schedule, Account accountOut, Account accountIn)
        {
            var firstFMove = schedule.FutureMoveList.First();

            firstFMove.Out = accountOut;
            firstFMove.In = accountIn;


            if (!schedule.Boundless)
            {
                var addedFMoveCount = schedule.FutureMoveList.Count;

                for (var fm = addedFMoveCount; fm < schedule.Times; fm++)
                {
                    addNextFutureMove(schedule);
                }
            }

            if (schedule.ShowInstallment)
            {
                var total = schedule.FutureMoveList.Count;

                var format = schedule.Boundless
                                 ? "{0} [{1}]"
                                 : "{0} [{1}/{2}]";

                for (var fm = 0; fm < total; fm++)
                {
                    schedule.FutureMoveList[fm].Description =
                        String.Format(format,
                                      schedule.FutureMoveList[fm].Description,
                                      fm + 1, total);
                }
            }


            foreach (var futureMove in schedule.FutureMoveList)
            {
                futureMoveService.SaveOrUpdate(futureMove);

                detailService.SaveDetails(futureMove);
            }


            scheduleService.SaveOrUpdate(schedule);


            return firstFMove;
        }



        public void TransformFutureInMove(FutureMove futureMove, bool boundless, Format.GetterForMove formatGetter)
        {
            if (boundless)
            {
                addNextFutureMove(futureMove.Schedule);
            }

            var move = futureMove.Cast();

            moneyService.SaveOrUpdateMove(move, futureMove.Out, futureMove.In, formatGetter);
            DeleteFutureMove(futureMove);
        }


        private void addNextFutureMove(Schedule schedule)
        {
            var nextDate = scheduleService.GetNextRunDate(schedule);

            var nextFMove = schedule.FutureMoveList.First().GetNext(nextDate);

            schedule.FutureMoveList.Add(nextFMove);
        }

        #endregion

    }
}
