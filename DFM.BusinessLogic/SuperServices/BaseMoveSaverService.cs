using System;
using Ak.Generic.Collection;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.SuperServices
{
    internal class BaseMoveSaverService : BaseSuperService
    {
        private readonly MoveService moveService;
        private readonly DetailService detailService;
        private readonly SummaryService summaryService;
        private readonly MonthService monthService;
        private readonly YearService yearService;

        internal BaseMoveSaverService(ServiceAccess serviceAccess, MoveService moveService, DetailService detailService, SummaryService summaryService, MonthService monthService, YearService yearService)
            : base(serviceAccess)
        {
            this.moveService = moveService;
            this.detailService = detailService;
            this.summaryService = summaryService;
            this.monthService = monthService;
            this.yearService = yearService;
        }

        
        
        internal Move SaveOrUpdateMove(Move move, String accountOutName, String accountInName, String categoryName)
        {
            var accountOut = selectAccountByName(accountOutName);
            var accountIn = selectAccountByName(accountInName);

            SetCategory(move, categoryName);

            resetSchedule(move);

            ajustOldSummaries(move.ID);

            placeAccountsInMove(move, accountOut, accountIn);

            move = moveService.SaveOrUpdate(move);

            detailService.SaveDetails(move);

            AjustSummaries(move);

            return move;
        }

        private Account selectAccountByName(String accountName)
        {
            return accountName == null
                       ? null
                       : Parent.Admin.SelectAccountByName(accountName);
        }

        private void resetSchedule(Move move)
        {
            if (move.ID == 0) return;

            var oldMove = moveService.SelectOldById(move.ID);

            move.Schedule = oldMove == null
                ? null : oldMove.Schedule;
        }

        internal void SetCategory(BaseMove baseMove, String categoryName)
        {
            if (categoryName != null)
                baseMove.Category = Parent.Admin.SelectCategoryByName(categoryName);
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

            AjustSummaries(oldMove);
        }

        internal void AjustSummaries(Move move)
        {
            if (move.Nature.IsIn(MoveNature.In, MoveNature.Transfer))
                ajustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccIn());

            if (move.Nature.IsIn(MoveNature.Out, MoveNature.Transfer))
                ajustSummary((Int16)move.Date.Month, (Int16)move.Date.Year, move.Category, move.AccOut());
        }

        private void ajustSummary(Int16 monthDate, Int16 yearDate, Category category, Account account)
        {
            ajustMonth(monthDate, yearDate, category, account);
            ajustYear(yearDate, category, account);
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
            if (baseMove.Date == DateTime.MinValue)
                return null;

            var year = yearService.GetOrCreateYear((Int16)baseMove.Date.Year, account, baseMove.Category);

            return monthService.GetOrCreateMonth((Int16)baseMove.Date.Month, year, baseMove.Category);
        }



        internal void SendEmail(Move move, OperationType operationType)
        {
            var emailAction = getEmailAction(operationType);

            moveService.SendEmail(move, emailAction);
        }

        private static string getEmailAction(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Creation:
                    return "create_move";
                case OperationType.Edit:
                    return "edit";
                case OperationType.Delete:
                    return "delete";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
