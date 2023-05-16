using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
	public class ReportService : Service
	{
		internal ReportService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public MonthReport GetMonthReport(String accountUrl, Int16 dateYear, Int16 dateMonth)
		{
			parent.Auth.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth <= 0 || dateMonth >= 13)
				throw Error.InvalidMonth.Throw();

			var user = parent.Auth.GetCurrent();
			var account = repos.Account.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = repos.Summary.GetTotal(account);

			var moveList = repos.Move
				.ByAccountAndTime(account, dateYear, dateMonth);

			var foreseenTotal = repos.Schedule
				.GetForeseenTotal(account, dateYear, dateMonth)
					+ total;

			var foreseenList = repos.Schedule
				.SimulateMoves(account, dateYear, dateMonth);

			var accountHasMoves = repos.Move.AccountHasMoves(account);

			return new MonthReport(
				account,
				total, foreseenTotal,
				moveList, foreseenList,
				accountHasMoves
			);
		}

		public YearReport GetYearReport(String accountUrl, Int16 dateYear)
		{
			parent.Auth.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var user = parent.Auth.GetCurrent();
			var account = repos.Account.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = repos.Summary.GetTotal(account);
			var months = repos.Summary.YearReport(account, dateYear);

			repos.Schedule.FillForeseenTotals(account, dateYear, months);
			var foreseen = repos.Schedule.GetForeseenTotal(account, dateYear);

			months = months.OrderBy(m => m.Number).ToList();

			return new YearReport(account, total, foreseen, dateYear, months);
		}

		public SearchResult SearchByDescription(String description)
		{
			parent.Auth.VerifyUser();

			if (String.IsNullOrEmpty(description))
				return new SearchResult();

			var email = parent.Current.Email;

			var terms = description.Split(" ");
			var movesWithTerm = repos.Move.ByDescription(email, terms);
			var detailsWithTerm = repos.Detail.ByDescription(email, terms);

			var moves = movesWithTerm
				.Union(detailsWithTerm.Select(d => d.Move))
				.ToList();

			return new SearchResult(moves);
		}

		public CategoryReport GetCategoryReport(String accountUrl, Int16 dateYear, Int16? dateMonth = null)
		{
			parent.Auth.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth is <= 0 or >= 13)
				throw Error.InvalidMonth.Throw();

			var user = parent.Auth.GetCurrent();
			var account = repos.Account.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();


			var nature = dateMonth.HasValue
				? SummaryNature.Month
				: SummaryNature.Year;

			var date = dateMonth.HasValue
				? dateYear * 100 + dateMonth.Value
				: dateYear;

			var summaryList = repos.Summary
				.Get(account, date);

			return new CategoryReport(nature, summaryList);
		}

	}
}
