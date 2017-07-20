using System;
using Ak.Generic.Collection;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class MoneyService
    {
        private readonly MoveService moveService;
        private readonly DetailService detailService;
        private readonly CategoryService categoryService;
        private readonly SummaryService summaryService;
        private readonly MonthService monthService;
        private readonly YearService yearService;
        private readonly ScheduleService scheduleService;

        internal MoneyService(MoveService moveService, DetailService detailService, CategoryService categoryService, SummaryService summaryService, MonthService monthService, YearService yearService, ScheduleService scheduleService)
        {
            this.moveService = moveService;
            this.detailService = detailService;
            this.categoryService = categoryService;
            this.summaryService = summaryService;
            this.monthService = monthService;
            this.yearService = yearService;
            this.scheduleService = scheduleService;
        }


        public Move SelectMoveById(Int32 id)
        {
            return moveService.SelectById(id);
        }



        #region Save or Update
        public Move SaveOrUpdateMove(Move move, Account accountOut, Account accountIn)
        {
            var transaction = moveService.BeginTransaction();
            
            try
            {
                move = SaveOrUpdateMoveWithOpenTransaction(move, accountOut, accountIn);

                moveService.CommitTransaction(transaction);
            }
            catch
            {
                moveService.RollbackTransaction(transaction);
                throw;
            }


            return move;
        }

        internal Move SaveOrUpdateMoveWithOpenTransaction(Move move, Account accountOut, Account accountIn)
        {
            var sendEmailAction = move.ID == 0 ? "create_move" : "edit";

            categoryService.SetCategory(move);

            ajustOldSummaries(move.ID);

            placeAccountsInMove(move, accountOut, accountIn);

            move = moveService.SaveOrUpdate(move);

            detailService.SaveDetails(move);

            ajustSummaries(move);

            moveService.SendEmail(move, sendEmailAction);

            return move;
        }

        #endregion



        public void DeleteMove(Move move)
        {
            var transaction = moveService.BeginTransaction();

            try
            {
                monthService.RemoveMoveFromMonth(move);
                ajustSummaries(move);

                moveService.Delete(move);

                moveService.SendEmail(move, "delete");

                move.Schedule.Times--;
                scheduleService.SaveOrUpdate(move.Schedule);

                moveService.CommitTransaction(transaction);
            }
            catch
            {
                moveService.RollbackTransaction(transaction);
                throw;
            }

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

        // ReSharper disable SuggestBaseTypeForParameter
        private void ajustSummaries(Move move)
        // ReSharper restore SuggestBaseTypeForParameter
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
            var year = yearService.GetOrCreateYear(yearDate, account);
            var month = monthService.GetOrCreateMonth(monthDate, year);

            var summaryMonth = month.GetOrCreateSummary(category);

            summaryService.AjustValue(summaryMonth);
        }

        private void ajustYear(Int16 yearDate, Category category, Account account)
        {
            var year = yearService.GetOrCreateYear(yearDate, account);

            var summaryYear = year.GetOrCreateSummary(category);

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
            var year = yearService.GetOrCreateYear((Int16)baseMove.Date.Year, account, baseMove.Category);
            return monthService.GetOrCreateMonth((Int16)baseMove.Date.Month, year, baseMove.Category);
        }

    }
}
