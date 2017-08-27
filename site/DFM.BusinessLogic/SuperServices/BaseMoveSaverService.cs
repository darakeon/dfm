using System;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.BusinessLogic.SuperServices
{
    internal class BaseMoveSaverService : BaseService
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

            var oldMove = moveService.GetOldById(move.ID);

            
            move = moveService.SaveOrUpdate(move);

            detailService.SaveDetails(move);

            
            if (oldMove != null)
                breakSummaries(oldMove);

            breakSummaries(move);


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



        public void BreakSummaries(Move move)
        {
            var oldMove = moveService.GetOldById(move.ID);

            breakSummaries(oldMove);
        }

        private void breakSummaries(Move move)
        {
            if (move.Nature != MoveNature.Out)
            {
                summaryService.CreateIfNotExists(move.In, move.Category);

                summaryService.Break(move.In, move.Category);
                summaryService.Break(move.In.Year, move.Category);
            }

            if (move.Nature != MoveNature.In)
            {
                summaryService.CreateIfNotExists(move.Out, move.Category);

                summaryService.Break(move.Out, move.Category);
                summaryService.Break(move.Out.Year, move.Category);
            }

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

        

        internal void FixSummaries()
        {
            BeginTransaction();

            try
            {
                var accounts = Parent.Admin.GetAccountsByUser(Parent.Current.User);

                foreach (var account in accounts)
                {
                    foreach (var year in account.YearList)
                    {
                        summaryService.Fix(year);

                        foreach (var month in year.MonthList)
                        {
                            summaryService.Fix(month);
                        }

                    }
                }

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }


        }


    }
}
