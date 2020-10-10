using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using Keon.NHibernate.Queries;
using monthItem = DFM.BusinessLogic.Response.YearReport.MonthItem;
using groupByMonth = Keon.NHibernate.Queries.GroupBy<DFM.Entities.Summary, System.Int64, DFM.BusinessLogic.Response.YearReport.MonthItem, System.Int32>;
using sumByMonth = Keon.NHibernate.Queries.Summarize<DFM.Entities.Summary, System.Int64, DFM.BusinessLogic.Response.YearReport.MonthItem, System.Int32>;
using groupByAccount = Keon.NHibernate.Queries.GroupBy<DFM.Entities.Summary, System.Int64, DFM.Entities.Summary, System.Int32>;
using sumByAccount = Keon.NHibernate.Queries.Summarize<DFM.Entities.Summary, System.Int64, DFM.Entities.Summary, System.Int32>;

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
				.TransformResult<Summary, Int32, groupByAccount, sumByAccount>(
					new List<groupByAccount>(),
					new[]
					{
						sumByAccount.GeS(s => s.InCents, s => s.InCents, SummarizeType.Sum),
						sumByAccount.GeS(s => s.OutCents, s => s.OutCents, SummarizeType.Sum)
					}
				)
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

		public IList<monthItem> YearReport(Account account, in short dateYear)
		{
			var yearBegin = new DateTime(dateYear, 1, 1);
			var yearEnd = new DateTime(dateYear, 12, 31);

			var group = groupByMonth.GeG(s => s.Time, m => m.Number);
			var sumIn = sumByMonth.GeS(s => s.InCents, m => m.CurrentInCents, SummarizeType.Sum);
			var sumOut = sumByMonth.GeS(s => s.OutCents, m => m.CurrentOutCents, SummarizeType.Sum);

			var query = NewQuery()
				.Where(
					s => s.Account.ID == account.ID
					     && s.Nature == SummaryNature.Month
					     && s.Time >= yearBegin.ToMonthYear()
					     && s.Time <= yearEnd.ToMonthYear()
				)
				.TransformResult<monthItem, Int32, groupByMonth, sumByMonth>(
					new [] {group},
					new [] {sumIn, sumOut}
				);

			return query.ResultAs<monthItem>();
		}
	}
}
