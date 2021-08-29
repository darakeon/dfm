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

			var accountHasMoves = repos.Move.AccountHasMoves(account);

			return new MonthReport(
				accountUrl,
				total, foreseenTotal,
				sign, foreseenSign,
				moveList, foreseenList,
				accountHasMoves
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

		private static Int16 countdown = (Int16)(Cfg.Tips.Countdown - 1);
		private static Int16 repeat = Cfg.Tips.Repeat;
		private static Int16 reset = (Int16)(Cfg.Tips.Reset - 1);

		public String ShowTip()
		{
			var tips = getTips();
			String result = null;

			if (tips.Countdown > 0)
			{
				tips.Countdown--;
			}
			else
			{
				if (tips.Repeat > 0)
				{
					tips.Repeat--;
					result = tips.LastGiven() ?? tips.Random();
				}

				if (tips.Repeat == 0)
				{
					resetTip(tips);
				}
			}

			inTransaction(
				"ShowTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);

			return result;
		}

		private Tips getTips()
		{
			var user = parent.Safe.VerifyUser();
			var type = parent.Current.TipType;

			return repos.Tips.By(user, type)
			    ?? createTip(user, type);
		}

		private static Tips createTip(User user, TipType type)
		{
			return new()
			{
				User = user,
				Type = type,
				Countdown = countdown,
				Repeat = repeat,
			};
		}

		private static void resetTip(Tips tips)
		{
			tips.Repeat = repeat;
			tips.Last = 0;

			if (tips.IsFull())
			{
				tips.Temporary = 0;
				tips.Countdown = reset;
			}
			else
			{
				tips.Countdown = countdown;
			}
		}

		public void DismissTip()
		{
			var tips = getTips();

			if (tips == null || tips.Last == 0)
				return;

			resetTip(tips);
			inTransaction(
				"DismissTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);
		}

		public void DisableTip(TipTests tip)
		{
			DisableTip((UInt64)tip);
		}

		public void DisableTip(TipBrowser tip)
		{
			DisableTip((UInt64)tip);
		}

		public void DisableTip(TipMobile tip)
		{
			DisableTip((UInt64)tip);
		}

		internal void DisableTip(UInt64 tip)
		{
			var tips = getTips();

			tips.Permanent += tip;

			inTransaction(
				"DisableTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);
		}
	}
}
