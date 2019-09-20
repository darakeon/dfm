using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
	public class ReportService : BaseService
	{
		private readonly AccountRepository accountRepository;
		private readonly MoveRepository moveRepository;
		private readonly SummaryRepository summaryRepository;

		internal ReportService(ServiceAccess serviceAccess, AccountRepository accountRepository, MoveRepository moveRepository, SummaryRepository summaryRepository)
			: base(serviceAccess)
		{
			this.accountRepository = accountRepository;
			this.moveRepository = moveRepository;
			this.summaryRepository = summaryRepository;
		}

		public MonthReport GetMonthReport(String accountUrl, Int16 dateMonth, Int16 dateYear)
		{
			Parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth <= 0 || dateMonth >= 13)
				throw Error.InvalidMonth.Throw();

			var user = Parent.Safe.GetCurrent();
			var account = accountRepository.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = summaryRepository.GetTotal(account);

			var moveList = moveRepository
				.ByAccountAndTime(account, dateYear, dateMonth);

			return new MonthReport(total, moveList);
		}

		public YearReport GetYearReport(String accountUrl, Int16 dateYear)
		{
			Parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var user = Parent.Safe.GetCurrent();
			var account = accountRepository.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = summaryRepository.GetTotal(account);

			var yearBegin = new DateTime(dateYear, 1, 1);
			var yearEnd = new DateTime(dateYear, 12, 31);

			// TODO: use summarize
			var summaries = summaryRepository
				.SimpleFilter(
					s => s.Account.ID == account.ID
						&& s.Nature == SummaryNature.Month
						&& s.Time >= yearBegin.ToMonthYear()
						&& s.Time <= yearEnd.ToMonthYear()
				);

			return new YearReport(total, dateYear, summaries);
		}
	}
}
