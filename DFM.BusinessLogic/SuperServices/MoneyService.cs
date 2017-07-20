using System;
using System.Linq;
using Ak.Generic.Collection;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class MoneyService
    {
        private readonly MoveService moveService;
        private readonly FutureMoveService futureMoveService;
        private readonly DetailService detailService;
        private readonly CategoryService categoryService;
        private readonly SummaryService summaryService;
        private readonly ScheduleService scheduleService;
        private readonly MonthService monthService;
        private readonly YearService yearService;

        internal MoneyService(MoveService moveService, FutureMoveService futureMoveService, DetailService detailService, CategoryService categoryService, SummaryService summaryService, ScheduleService scheduleService, MonthService monthService, YearService yearService)
        {
            this.moveService = moveService;
            this.futureMoveService = futureMoveService;
            this.detailService = detailService;
            this.categoryService = categoryService;
            this.summaryService = summaryService;
            this.scheduleService = scheduleService;
            this.monthService = monthService;
            this.yearService = yearService;
        }


        public Move SelectMoveById(Int32 id)
        {
            return moveService.SelectById(id);
        }



        #region Save or Update
        public FutureMove SaveOrUpdateSchedule(FutureMove futureMove, Account accountOut, Account accountIn)
        {
            if (futureMove.Schedule == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleRequired);

            var transaction = futureMoveService.BeginTransaction();

            try
            {
                categoryService.SetCategory(futureMove);

                if (!futureMove.Schedule.FutureMoveList.Any())
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

                for(var fm = addedFMoveCount; fm < schedule.Times; fm++)
                {
                    var nextDate = scheduleService.GetNextRunDate(schedule);
                    
                    var nextFMove = firstFMove.GetNext(nextDate);
                    
                    schedule.FutureMoveList.Add(nextFMove);
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



        public Move SaveOrUpdateMove(Move move, Account accountOut, Account accountIn, Format.GetterForMove getterForMove)
        {
            var sendEmailAction = move.ID == 0 ? "create_move" : "edit";

            var transaction = futureMoveService.BeginTransaction();

            try
            {
                categoryService.SetCategory(move);

                ajustOldSummaries(move.ID);

                placeAccountsInMove(move, accountOut, accountIn);

                move = moveService.SaveOrUpdate(move);

                detailService.SaveDetails(move);

                ajustSummaries(move);

                moveService.SendEmail(move, getterForMove, sendEmailAction);


                futureMoveService.CommitTransaction(transaction);
            }
            catch (Exception)
            {
                futureMoveService.RollbackTransaction(transaction);
                throw;
            }


            return move;
        }
        #endregion



        public void DeleteMove(Move move, Format.GetterForMove getterForMove)
        {
            monthService.RemoveMoveFromMonth(move);
            ajustSummaries(move);

            moveService.Delete(move);

            moveService.SendEmail(move, getterForMove, "delete");
        }

        public void DeleteMove(FutureMove move)
        {
            futureMoveService.Delete(move);
        }



        public Detail SelectDetailById(Int32 id)
        {
            return detailService.SelectById(id);
        }



        #region Ajust Summaries
        private void ajustOldSummaries(Int32 moveID)
        {
            var oldMove = moveService.SelectOldById(moveID);

            if (oldMove == null) return;

            if (oldMove.Nature != MoveNature.Out)
                oldMove.RemoveFromIn();

            if (oldMove.Nature != MoveNature.In)
                oldMove.RemoveFromOut();

            ajustSummaries(oldMove);
        }

        private void ajustSummaries(Move move)
        {
            if (move.Nature.IsIn(MoveNature.In, MoveNature.Transfer))
                ajustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccIn());

            if (move.Nature.IsIn(MoveNature.Out, MoveNature.Transfer))
                ajustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccOut());
        }

        private void ajustSummary(Int16 month, Int16 year, Category category, Account account)
        {
            ajustMonth(month, year, category, account);
            ajustYear(year, category, account);
        }

        private void ajustMonth(Int16 monthDate, Int16 yearDate, Category category, Account account)
        {
            var year = yearService.GetOrCreateYear(yearDate, account, summaryService.Delete);
            var month = monthService.GetOrCreateMonth(monthDate, year, summaryService.Delete);

            var summaryMonth = month.GetOrCreateSummary(category, summaryService.Delete);

            summaryService.AjustValue(summaryMonth);
        }

        private void ajustYear(Int16 yearDate, Category category, Account account)
        {
            var year = yearService.GetOrCreateYear(yearDate, account, summaryService.Delete);

            var summaryYear = year.GetOrCreateSummary(category, summaryService.Delete);

            summaryService.AjustValue(summaryYear);
        }
        #endregion



        private void placeAccountsInMove(Move move, Account accountOut, Account accountIn)
        {
            var monthOut = accountOut == null ? null : getMonth(move, accountOut);
            var monthIn = accountIn == null ? null : getMonth(move, accountIn);

            moveService.PlaceMonthsInMove(move, monthOut, monthIn);
        }

        private Month getMonth(BaseMove baseMove, Account account)
        {
            var year = yearService.GetOrCreateYear((Int16)baseMove.Date.Year, account, summaryService.Delete, baseMove.Category);
            return monthService.GetOrCreateMonth((Int16)baseMove.Date.Month, year, summaryService.Delete, baseMove.Category);
        }

    }
}
