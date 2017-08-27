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
        private readonly AccountRepository accountRepository;
        private readonly YearRepository yearRepository;
        private readonly MonthRepository monthRepository;

        internal ReportService(ServiceAccess serviceAccess, AccountRepository accountRepository, YearRepository yearRepository, MonthRepository monthRepository)
            : base(serviceAccess)
        {
            this.accountRepository = accountRepository;
            this.monthRepository = monthRepository;
            this.yearRepository = yearRepository;
        }



        public IList<Move> GetMonthReport(String accountUrl, Int16 dateMonth, Int16 dateYear)
        {
            VerifyUser();

            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            if (dateMonth <= 0 || dateMonth >= 13)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMonth);

            var account = accountRepository.GetByUrl(accountUrl, Parent.Current.User);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            var year = yearRepository.GetOrCreateYear(dateYear, account);

            if (year == null)
                return new List<Move>();


            var month = monthRepository.GetOrCreateMonth(dateMonth, year);

            return month == null
                ? new List<Move>()
                : month.MoveList();
        }

        
        
        public Year GetYearReport(String accountUrl, Int16 dateYear)
        {
            VerifyUser();

            if (dateYear <= 0)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidYear);

            var account = accountRepository.GetByUrl(accountUrl, Parent.Current.User);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            var year = yearRepository.GetOrCreateYear(dateYear, account);

            return accountRepository.NonFuture(year);
        }




    }
}
