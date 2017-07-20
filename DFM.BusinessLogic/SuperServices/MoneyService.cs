using System;
using Ak.Generic.Collection;
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
        private readonly SummaryService summaryService;
        private readonly ScheduleService scheduleService;
        private readonly MonthService monthService;
        private readonly YearService yearService;

        internal MoneyService(MoveService moveService, FutureMoveService futureMoveService, DetailService detailService, SummaryService summaryService, ScheduleService scheduleService, MonthService monthService, YearService yearService)
        {
            this.moveService = moveService;
            this.futureMoveService = futureMoveService;
            this.yearService = yearService;
            this.monthService = monthService;
            this.scheduleService = scheduleService;
            this.summaryService = summaryService;
            this.detailService = detailService;
        }


        public Move SelectMoveById(Int32 id)
        {
            return moveService.SelectById(id);
        }



        public FutureMove SaveOrUpdateMove(FutureMove move, Account accountOut, Account accountIn)
        {
            scheduleService.AjustSchedule(move);

            move.Out = accountOut;
            move.In = accountIn;

            move = futureMoveService.SaveOrUpdate(move);

            ajustDetailAndSummaries(move);

            return move;
        }

        public Move SaveOrUpdateMove(Move move, Account accountOut, Account accountIn, Format.GetterForMove getterForMove)
        {
            ajustOldSummaries(move.ID);

            placeAccountsInMove(move, accountOut, accountIn);

            move = moveService.SaveOrUpdate(move);

            ajustDetailAndSummaries(move);

            var action = move.ID == 0 ? "create_move" : "edit"; 
            moveService.SendEmail(move, getterForMove, action);

            return move;
        }

        private void ajustDetailAndSummaries(BaseMove move)
        {
            foreach (var detail in move.DetailList)
            {
                detailService.SaveOrUpdate(detail);
            }

            ajustSummaries(move);
        }


        public void DeleteMove(Move move, Format.GetterForMove getterForMove)
        {
            monthService.RemoveMoveFromMonth(move);
            ajustSummaries(move);

            moveService.Delete(move);

            moveService.SendEmail(move, getterForMove, "delete");
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

            if (oldMove.In != null)
                oldMove.RemoveFromIn();

            if (oldMove.Out != null)
                oldMove.RemoveFromOut();

            ajustSummaries(oldMove);
        }

        private void ajustSummaries(BaseMove move)
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

        private Month getMonth(BaseMove move, Account account)
        {
            var year = yearService.GetOrCreateYear((Int16)move.Date.Year, account, summaryService.Delete, move.Category);
            return monthService.GetOrCreateMonth((Int16)move.Date.Month, year, summaryService.Delete, move.Category);
        }

    }
}
