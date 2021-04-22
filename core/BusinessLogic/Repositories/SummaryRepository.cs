using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic.Datetime;
using monthItem = DFM.BusinessLogic.Response.YearReport.MonthItem;

namespace DFM.BusinessLogic.Repositories
{
	internal class SummaryRepository : Repo<Summary>
	{
		internal void Break(Account account, Category category, IDate date)
		{
			var year = date.Year;
			var month = date.ToMonthYear();

			@break(account, category, year, SummaryNature.Year);
			@break(account, category, month, SummaryNature.Month);
		}

		private void @break(Account account, Category category, Int32 time, SummaryNature nature)
		{
			var summary = Get(account, category, time);

			if (summary == null)
				create(account, category, time, nature);
			else
				@break(summary);
		}

		private void create(Account account, Category category, Int32 time, SummaryNature nature)
		{
			var summary = new Summary
			{
				Account = account,
				Category = category,
				Time = time,
				Nature = nature,
				Broken = true,
			};

			SaveOrUpdate(summary);
		}

		private void @break(Summary summary)
		{
			summary.Broken = true;
			SaveOrUpdate(summary);
		}

		public void Fix(Summary summary, Decimal @in, Decimal @out)
		{
			summary.In = @in;
			summary.Out = @out;
			summary.Broken = false;

			SaveOrUpdate(summary);
		}

		internal Summary Get(Account account, Category category, Int32 time)
		{
			return Get(account, time)
				.SingleOrDefault(s => s.Category.Is(category));
		}

		internal IList<Summary> Get(Account account, Int32 time)
		{
			return Where(
				s => s.Account.ID == account.ID
				     && s.Time == time
			);
		}

		internal Decimal GetTotal(Account account)
		{
			var result = NewQuery()
				.Where(s => s.Account.ID == account.ID)
				.Where(s => s.Nature == SummaryNature.Year)
				.TransformResult<Summary>()
				.Sum(s => s.InCents, s => s.InCents)
				.Sum(s => s.OutCents, s => s.OutCents)
				.SingleOrDefault;

			var value = result.InCents - result.OutCents;

			return value/100m;
		}

		public void DeleteAll(Account account)
		{
			Where(s => s.Account.ID == account.ID)
				.ToList()
				.ForEach(Delete);
		}

		public IList<monthItem> YearReport(Account account, Int16 dateYear)
		{
			var yearBegin = new DateTime(dateYear, 1, 1);
			var yearEnd = new DateTime(dateYear, 12, 31);

			var months = NewQuery()
				.Where(
					s => s.Account.ID == account.ID
					     && s.Nature == SummaryNature.Month
					     && s.Time >= yearBegin.ToMonthYear()
					     && s.Time <= yearEnd.ToMonthYear()
				)
				.TransformResult<monthItem>()
				.GroupBy(s => s.Time, m => m.Number)
				.Sum(s => s.InCents, m => m.CurrentInCents)
				.Sum(s => s.OutCents, m => m.CurrentOutCents)
				.List;

			return months;
		}
	}
}
