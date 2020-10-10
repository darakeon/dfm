using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
	public class ReportService : Service
	{
		private readonly AccountRepository accountRepository;
		private readonly MoveRepository moveRepository;
		private readonly DetailRepository detailRepository;
		private readonly SummaryRepository summaryRepository;

		internal ReportService(ServiceAccess serviceAccess, AccountRepository accountRepository, MoveRepository moveRepository, DetailRepository detailRepository, SummaryRepository summaryRepository)
			: base(serviceAccess)
		{
			this.accountRepository = accountRepository;
			this.moveRepository = moveRepository;
			this.detailRepository = detailRepository;
			this.summaryRepository = summaryRepository;
		}

		public MonthReport GetMonthReport(String accountUrl, Int16 dateMonth, Int16 dateYear)
		{
			parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth <= 0 || dateMonth >= 13)
				throw Error.InvalidMonth.Throw();

			var user = parent.Safe.GetCurrent();
			var account = accountRepository.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = summaryRepository.GetTotal(account);

			var moveList = moveRepository
				.ByAccountAndTime(account, dateYear, dateMonth);

			return new MonthReport(moveList, accountUrl, total);
		}

		public YearReport GetYearReport(String accountUrl, Int16 dateYear)
		{
			parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var user = parent.Safe.GetCurrent();
			var account = accountRepository.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = summaryRepository.GetTotal(account);

			var yearBegin = new DateTime(dateYear, 1, 1);
			var yearEnd = new DateTime(dateYear, 12, 31);

			// TODO: use summarize
			var summaries = summaryRepository
				.Where(
					s => s.Account.ID == account.ID
						&& s.Nature == SummaryNature.Month
						&& s.Time >= yearBegin.ToMonthYear()
						&& s.Time <= yearEnd.ToMonthYear()
				);

			return new YearReport(total, dateYear, summaries);
		}

		public SearchResult SearchByDescription(String description)
		{
			parent.Safe.VerifyUser();

			if (String.IsNullOrEmpty(description))
				return new SearchResult();

			var email = parent.Current.Email;

			var terms = description.Split(" ");
			var movesWithTerm = moveRepository.ByDescription(email, terms);
			var detailsWithTerm = detailRepository.ByDescription(email, terms);

			var moves = movesWithTerm
				.Union(detailsWithTerm.Select(d => d.Move))
				.ToList();

			return new SearchResult(moves);
		}
	}
}
