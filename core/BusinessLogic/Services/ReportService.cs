using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Services
{
	public class ReportService : Service
	{
		internal ReportService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public MonthReport GetMonthReport(String accountUrl, Int16 dateYear, Int16 dateMonth)
		{
			parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth <= 0 || dateMonth >= 13)
				throw Error.InvalidMonth.Throw();

			var user = parent.Safe.GetCurrent();
			var account = repos.Account.GetByUrl(accountUrl, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			var total = repos.Summary.GetTotal(account);
			var sign = account.GetSign(total);

			var moveList = repos.Move
				.ByAccountAndTime(account, dateYear, dateMonth);

			var foreseenTotal = repos.Schedule
				.GetForeseenTotal(account, dateYear, dateMonth)
					+ total;
			var foreseenSign = account.GetSign(foreseenTotal);

			var foreseenList = repos.Schedule
				.SimulateMoves(account, dateYear, dateMonth);

			return new MonthReport(
				accountUrl,
				total, foreseenTotal,
				sign, foreseenSign,
				moveList, foreseenList
			);
		}

		public YearReport GetYearReport(String accountUrl, Int16 dateYear)
		{
			parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			var user = parent.Safe.GetCurrent();
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
			parent.Safe.VerifyUser();

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
			parent.Safe.VerifyUser();

			if (dateYear <= 0)
				throw Error.InvalidYear.Throw();

			if (dateMonth is <= 0 or >= 13)
				throw Error.InvalidMonth.Throw();

			var user = parent.Safe.GetCurrent();
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
