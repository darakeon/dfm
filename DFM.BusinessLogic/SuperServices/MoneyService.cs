using System;
using Ak.Generic.Collection;
using DFM.BusinessLogic.Services;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class MoneyService
    {
        private readonly MoveService moveService;
        private readonly DetailService detailService;
        private readonly SummaryService summaryService;
        private readonly ScheduleService scheduleService;
        private readonly MonthService monthService;
        private readonly YearService yearService;

        internal MoneyService(MoveService moveService, DetailService detailService, SummaryService summaryService, ScheduleService scheduleService, MonthService monthService, YearService yearService)
        {
            this.moveService = moveService;
            this.yearService = yearService;
            this.monthService = monthService;
            this.scheduleService = scheduleService;
            this.summaryService = summaryService;
            this.detailService = detailService;
        }


        public Move SaveOrUpdateMove(Move move, Account accountOut, Account accountIn, Format.GetterForMove getterForMove)
        {
            var oldMove = moveService.SelectOldById(move.ID);
            AjustOldSummaries(oldMove);

            placeAccountsInMove(move, accountOut, accountIn);

            scheduleService.AjustSchedule(move);

            move = moveService.SaveOrUpdate(move);

            foreach (var detail in move.DetailList)
            {
                detailService.SaveOrUpdate(detail);
            }

            AjustSummaries(move);

            var action = move.ID == 0 ? "create_move" : "edit";
            moveService.SendEmail(move, getterForMove, action);

            return move;
        }

        public void DeleteMove(Move move, Format.GetterForMove getterForMove)
        {
            monthService.RemoveMoveFromMonth(move);
            AjustSummaries(move);

            moveService.Delete(move);

            moveService.SendEmail(move, getterForMove, "delete");
        }

        private Month getMonth(Move move, Account account)
        {
            var year = yearService.GetOrCreateYear((Int16)move.Date.Year, account, summaryService.Delete, move.Category);
            return monthService.GetOrCreateMonth((Int16)move.Date.Month, year, summaryService.Delete, move.Category);
        }

        private void placeAccountsInMove(Move move, Account accountOut, Account accountIn)
        {
            var monthOut = accountOut == null ? null : getMonth(move, accountOut);
            var monthIn = accountIn == null ? null : getMonth(move, accountIn);

            moveService.PlaceMonthsInMove(move, monthOut, monthIn);
        }


        internal void AjustOldSummaries(Move oldMove)
        {
            if (oldMove == null) return;

            if (oldMove.In != null)
                oldMove.RemoveFromIn();

            if (oldMove.Out != null)
                oldMove.RemoveFromOut();

            AjustSummaries(oldMove);
        }

        internal void AjustSummaries(Move move)
        {
            if (move.Nature.IsIn(MoveNature.In, MoveNature.Transfer))
                AjustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccountIn());

            if (move.Nature.IsIn(MoveNature.Out, MoveNature.Transfer))
                AjustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccountOut());
        }

        internal void AjustSummary(Int16 month, Int16 year, Category category, Account account)
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


    }
}
