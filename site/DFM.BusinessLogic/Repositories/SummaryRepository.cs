using System;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Extensions;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class SummaryRepository : BaseRepositoryLong<Summary>
	{
		internal void Break(Year year, Category category)
		{
			var summary = getByYearAndCategory(year, category);

			@break(summary);
		}

		private Summary getByYearAndCategory(Year year, Category category)
		{
			return SimpleFilter(
				s => s.Year != null
				&& s.Year.ID == year.ID
			).SingleOrDefault(s => s.Category.Is(category));
		}

		internal void Break(Month month, Category category)
		{
			var summary = getByMonthAndCategory(month, category);

			@break(summary);
		}

		private Summary getByMonthAndCategory(Month month, Category category)
		{
			return SimpleFilter(
				s => s.Month != null 
				&& s.Month.ID == month.ID
			).SingleOrDefault(s => s.Category.Is(category));
		}

		private void @break(Summary summary)
		{
			summary.Broken = true;

			if (summary.ID != 0)
				SaveOrUpdate(summary);
		}



		public void Fix(Summary summary, Decimal @in, Decimal @out)
		{
			summary.In = @in;
			summary.Out = @out;
			summary.Broken = false;

			SaveOrUpdate(summary);
		}



		internal void CreateIfNotExists(Month month, Category category)
		{
			if (month == null)
				return;

			createForMonthIfNotExists(month, category);
			createForYearIfNotExists(month.Year, category);
		}

		private void createForMonthIfNotExists(Month month, Category category)
		{
			var summaryMonth = getByMonthAndCategory(month, category);

			if (summaryMonth == null)
			{
				summaryMonth = new Summary(month, category);

				SaveOrUpdate(summaryMonth);
			}
		}

		private void createForYearIfNotExists(Year year, Category category)
		{
			var summaryYear = getByYearAndCategory(year, category);

			if (summaryYear == null)
			{
				summaryYear = new Summary(year, category);

				SaveOrUpdate(summaryYear);
			}
		}



	}
}
