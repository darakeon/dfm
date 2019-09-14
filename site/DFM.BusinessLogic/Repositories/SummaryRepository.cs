using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class SummaryRepository : BaseRepositoryLong<Summary>
	{
		internal void Break(Account account, Category category, DateTime date)
		{
			var year = date.Year;
			var month = year * 100 + date.Month;

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
			return SimpleFilter(
				s => s.Account.ID == account.ID
				     && s.Time == time
			);
		}

		internal Decimal GetTotal(Account account)
		{
			// TODO: refactor to use summarize
			return SimpleFilter(s => s.Account.ID == account.ID)
				.Where(s => s.Nature == SummaryNature.Year)
				.Sum(s => s.Value());
		}

		public void DeleteAll(Account account)
		{
			SimpleFilter(s => s.Account.ID == account.ID)
				.ToList()
				.ForEach(Delete);
		}
	}
}
