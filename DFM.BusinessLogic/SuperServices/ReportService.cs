using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class ReportService
    {
        private readonly AccountService accountService;
        private readonly YearService yearService;
        private readonly MonthService monthService;

        internal ReportService(AccountService accountService, YearService yearService, MonthService monthService)
        {
            this.accountService = accountService;
            this.monthService = monthService;
            this.yearService = yearService;
        }



        public IList<Move> GetMonthReport(Int32 accountID, Int16 dateMonth, Int16 dateYear)
        {
            var account = accountService.SelectById(accountID);


            var year = yearService.GetOrCreateYear(dateYear, account);

            if (year == null)
                return new List<Move>();


            var month = monthService.GetOrCreateMonth(dateMonth, year);

            return month == null
                ? new List<Move>()
                : month.MoveList();
        }

        
        
        public Year GetYearReport(Int32 accountID, Int16 dateYear)
        {
            var account = accountService.SelectById(accountID);

            var year = yearService.GetOrCreateYear(dateYear, account);

            return accountService.NonFuture(year);
        }




    }
}
