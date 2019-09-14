using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;

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

			return moveRepository
				.ByAccountAndTime(account, dateYear, dateMonth);
		}



		public YearReport GetYearReport(String accountUrl, Int16 dateYear)
		{
			Parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var account = accountRepository.GetByUrl(accountUrl, Parent.Current.User);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			// TODO: use summarize
			var summaries = summaryRepository
				.SimpleFilter(
					s => s.Account.ID == account.ID
						&& s.Nature == SummaryNature.Month
						&& s.Time > dateYear * 100
						&& s.Time < (dateYear + 1) * 100
				);

			return new YearReport(dateYear, summaries);
		}
	}
}
