using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;

namespace DFM.BusinessLogic.Services
{
	public class ReportService : Service
	{
		internal ReportService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public MonthReport GetMonthReport(String accountUrl, Int16 dateMonth, Int16 dateYear)
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

			var foreseenTotal = repos.Schedule
				.GetForeseenTotal(account, dateYear, dateMonth);

			var moveList = repos.Move
				.ByAccountAndTime(account, dateYear, dateMonth);

			var foreseenList = repos.Schedule
				.SimulateMoves(account, dateYear, dateMonth);

			return new MonthReport(accountUrl, total, foreseenTotal, moveList, foreseenList);
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

			return new YearReport(total, foreseen, dateYear, months);
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
	}
}
