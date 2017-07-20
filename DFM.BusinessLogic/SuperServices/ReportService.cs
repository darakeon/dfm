using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class ReportService
    {
        private readonly AccountService accountService;
        private readonly YearService yearService;
        private readonly MonthService monthService;
        private readonly SummaryService summaryService;

        internal ReportService(AccountService accountService, YearService yearService, MonthService monthService, SummaryService summaryService)
        {
            this.accountService = accountService;
            this.summaryService = summaryService;
            this.monthService = monthService;
            this.yearService = yearService;
        }

        public IList<Move> GetMonthReport(Int32 id, Int16 dateMonth, Int16 dateYear)
        {
            var account = accountService.SelectById(id);


            var year = yearService.GetOrCreateYear(dateYear, account, summaryService.Delete);

            if (year == null)
                return new List<Move>();


            var month = monthService.GetOrCreateMonth(dateMonth, year, summaryService.Delete);

            return month == null
                ? new List<Move>()
                : month.MoveList();
        }

        public Year GetYearReport(Int32 accountid, Int16 dateYear)
        {
            var account = accountService.SelectById(accountid);

            var year = yearService.GetOrCreateYear(dateYear, account, summaryService.Delete);

            return accountService.NonFuture(year);
        }




    }
}
