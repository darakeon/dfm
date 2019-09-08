using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;

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
			Parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth <= 0 || dateMonth >= 13)
				throw Error.InvalidMonth.Throw();

			var account = accountRepository.GetByUrl(accountUrl, Parent.Current.User);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var year = yearRepository.GetOrCreateYear(dateYear, account);

			if (year == null)
				return new List<Move>();


			var month = monthRepository.GetOrCreateMonth(dateMonth, year);

			if (month == null)
				return new List<Move>();

			return month.MoveList()
				.OrderBy(m => m.Date)
				.ToList();
		}



		public Year GetYearReport(String accountUrl, Int16 dateYear)
		{
			Parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var account = accountRepository.GetByUrl(accountUrl, Parent.Current.User);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var year = yearRepository.GetOrCreateYear(dateYear, account);

			return accountRepository.NonFuture(year);
		}




	}
}
