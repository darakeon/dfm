using System;
using System.Linq;
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
            var accountOut = getAccountByName(accountOutName);
            var accountIn = getAccountByName(accountInName);

            
            SetCategory(move, categoryName);

            resetSchedule(move);

            
            var oldMove = getOldAndRemoveFromMonths(move);

            placeAccountsInMove(move, accountOut, accountIn);

            
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

        internal void SetCategory(BaseMove baseMove, String categoryName)
        {
            if (categoryName != null)
                baseMove.Category = Parent.Admin.GetCategoryByName(categoryName);
        }

        private void resetSchedule(Move move)
        {
            if (move.ID == 0) return;

            var oldMove = moveService.GetOldById(move.ID);

            move.Schedule = oldMove == null
                ? null : oldMove.Schedule;
        }



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



        public void RemoveFromSummaries(Move move)
        {
            var oldMove = getOldAndRemoveFromMonths(move);
            ajustSummaries(oldMove);
        }

        private Move getOldAndRemoveFromMonths(Move move)
        {
            var oldMove = moveService.GetOldById(move.ID);

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

                var changed = changedYear || changedMonth || changedCategory || changedAccounts;

                if (!changed)
                    return;

                checkUpYear = changedCategory || changedYear || changedAccounts;
            }

            if (move.Nature != MoveNature.Out)
            {
                summaryService.AjustValue(move.In[move.Category.Name]);
                
                if (checkUpYear)
                    summaryService.AjustValue(move.In.Year[move.Category.Name]);
            }

            if (move.Nature != MoveNature.In)
            {
                summaryService.AjustValue(move.Out[move.Category.Name]);

                if (checkUpYear)
                    summaryService.AjustValue(move.Out.Year[move.Category.Name]);
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
