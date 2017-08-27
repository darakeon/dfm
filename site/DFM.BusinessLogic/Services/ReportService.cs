using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    public class ReportService : BaseService
    {
        private readonly AccountRepository accountService;
        private readonly YearRepository yearService;
        private readonly MonthRepository monthService;

        internal ReportService(ServiceAccess serviceAccess, AccountRepository accountService, YearRepository yearService, MonthRepository monthService)
            : base(serviceAccess)
        {
            this.accountService = accountService;
            this.monthService = monthService;
            this.yearService = yearService;
        }



        public IList<Move> GetMonthReport(String accountUrl, Int16 dateMonth, Int16 dateYear)
        {
            VerifyUser();

            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            if (dateMonth <= 0 || dateMonth >= 13)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMonth);

            var account = accountService.GetByUrl(accountUrl, Parent.Current.User);

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

        
        
        public Year GetYearReport(String accountUrl, Int16 dateYear)
        {
            VerifyUser();

            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            var account = accountService.GetByUrl(accountUrl, Parent.Current.User);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            var year = yearService.GetOrCreateYear(dateYear, account);

            return accountService.NonFuture(year);
        }




    }
}
