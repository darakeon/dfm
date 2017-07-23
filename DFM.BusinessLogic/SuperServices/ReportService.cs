using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.SuperServices
{
    public class ReportService : BaseSuperService
    {
        private readonly AccountService accountService;
        private readonly YearService yearService;
        private readonly MonthService monthService;

        internal ReportService(ServiceAccess serviceAccess, AccountService accountService, YearService yearService, MonthService monthService)
            : base(serviceAccess)
        {
            this.accountService = accountService;
            this.monthService = monthService;
            this.yearService = yearService;
        }



        public IList<Move> GetMonthReport(Int32 accountID, Int16 dateMonth, Int16 dateYear)
        {
            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            if (dateMonth <= 0 || dateMonth >= 13)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMonth);

            var account = accountService.SelectById(accountID);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

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
            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            var account = accountService.SelectById(accountID);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            var year = yearService.GetOrCreateYear(dateYear, account);

            return accountService.NonFuture(year);
        }




    }
}
