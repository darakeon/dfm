using System;
using DFM.BusinessLogic.Services;
using DFM.Entities;
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
            var accountOut = getAccountByName(accountOutName);
            var accountIn = getAccountByName(accountInName);

            var category = Parent.Admin.GetCategoryByName(categoryName);


            resetSchedule(move);

            
            linkEntities(move, accountOut, accountIn, category);

            var oldMove = getOldAndRemoveFromMonths(move, true);

            
            move = moveService.SaveOrUpdate(move);

            detailService.SaveDetails(move);

            
            if (oldMove != null)
                ajustSummaries(oldMove, move);

            ajustSummaries(move);


            return move;
        }

        private Account getAccountByName(String accountName)
        {
            return accountName == null
                       ? null
                       : Parent.Admin.GetAccountByName(accountName);
        }

        private void resetSchedule(Move move)
        {
            if (move.ID == 0) return;

            var oldMove = moveService.GetOldById(move.ID);

            move.Schedule = oldMove == null
                ? null : oldMove.Schedule;
        }



        private void linkEntities(Move move, Account accountOut, Account accountIn, Category category)
        {
            move.Category = category;

            var monthOut = accountOut == null ? null : getMonth(move, accountOut);
            var monthIn = accountIn == null ? null : getMonth(move, accountIn);

            moveService.PlaceMonthsInMove(move, monthOut, monthIn);
        }

        private Month getMonth(Move move, Account account)
        {
            if (move.Date == DateTime.MinValue)
                return null;

            var year = yearService.GetOrCreateYear((Int16)move.Date.Year, account, move.Category);

            return monthService.GetOrCreateMonth((Int16)move.Date.Month, year, move.Category);
        }



        public void RemoveFromSummaries(Move move)
        {
            var oldMove = getOldAndRemoveFromMonths(move, false);

            ajustSummaries(oldMove);
        }

        private Move getOldAndRemoveFromMonths(Move move, Boolean verifyBeforeRemove)
        {
            var oldMove = moveService.GetOldById(move.ID);

            if (oldMove == null)
                return null;

            if (verifyBeforeRemove)
            {
                var changedCategory = move.Category.Name != oldMove.Category.Name;

                var changedMonth = move.Date.Month != oldMove.Date.Month;

                var changedYear = move.Date.Year != oldMove.Date.Year;

                var changedAccounts = testChangedAccounts(move, oldMove);

                var changedParent = changedAccounts || changedYear || changedMonth;
                var changed = changedParent || changedCategory;

                if (!changed)
                    return null;

                if (!changedParent)
                    return oldMove;
            }

            monthService.RemoveMoveFromMonth(oldMove);

            return oldMove;
        }

        private void ajustSummaries(Move move, Move changedMove = null)
        {
            var checkUpYear = true;

            if (changedMove != null)
            {
                var changedCategory = move.Category.Name != changedMove.Category.Name;
                
                var changedMonth = move.Date.Month != changedMove.Date.Month;

                var changedYear = move.Date.Year != changedMove.Date.Year;

                var changedAccounts = testChangedAccounts(move, changedMove);

                var changed = changedAccounts || changedYear || changedMonth || changedCategory;

                if (!changed)
                    return;

                checkUpYear = changedAccounts || changedYear || changedCategory;
            }

            if (move.Nature != MoveNature.Out)
            {
                summaryService.AjustValue(move.In.GetOrCreateSummary(move.Category));
                
                if (checkUpYear)
                    summaryService.AjustValue(move.In.Year.GetOrCreateSummary(move.Category));
            }

            if (move.Nature != MoveNature.In)
            {
                summaryService.AjustValue(move.Out.GetOrCreateSummary(move.Category));

                if (checkUpYear)
                    summaryService.AjustValue(move.Out.Year.GetOrCreateSummary(move.Category));
            }

        }

        private Boolean testChangedAccounts(Move move, Move changedMove)
        {
            if (move.Nature != changedMove.Nature)
                return true;

            var changed = false;

            if (move.Nature != MoveNature.Out)
                changed |= move.AccIn().ID != changedMove.AccIn().ID;

            if (move.Nature != MoveNature.In)
                changed |= move.AccOut().ID != changedMove.AccOut().ID;

            return changed;
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
