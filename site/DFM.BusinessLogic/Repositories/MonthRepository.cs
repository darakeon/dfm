using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Repositories
{
	internal class MonthRepository : BaseRepository<Month>
	{
		internal Month GetOrCreateMonthWithSummary(Int16 dateMonth, Year year, Category category)
		{
			var month = GetOrCreateMonth(dateMonth, year);

			month.GetOrCreateSummary(category);

			return month;
		}

		internal Month GetOrCreateMonth(Int16 dateMonth, Year year)
		{
			var month = year.MonthList
				.SingleOrDefault(m => m.Time == dateMonth);

			return month ?? createMonth(year, dateMonth);
		}

		private Month createMonth(Year year, Int16 month)
		{
			var newMonth = new Month { Year = year, Time = month };

			year.MonthList.Add(newMonth);

			SaveOrUpdate(newMonth);

			return newMonth;
		}


		internal void SaveOrUpdate(Month month)
		{
			base.SaveOrUpdate(month);
		}

		internal void RemoveMoveFromMonth(Move move)
		{
			if (move.In != null)
			{
				move.In.InList.Remove(move);
				SaveOrUpdate(move.In);
			}

			if (move.Out != null)
			{
				move.Out.OutList.Remove(move);
				SaveOrUpdate(move.Out);
			}
		}



		internal Decimal GetIn(Year year, Category category)
		{
			return get(year, category).Sum(m => m.In);
		}

		internal Decimal GetOut(Year year, Category category)
		{
			return get(year, category).Sum(m => m.Out);
		}

		private IEnumerable<Summary> get(Year year, Category category)
		{
			var summaryList = NewQuery()
				.SimpleFilter(m => m.Year.ID == year.ID)
				.Result
				.SelectMany(m => m.SummaryList);

			summaryList = category == null
				? summaryList.Where(m => m.Category == null)
				: summaryList.Where(m => m.Category != null && m.Category.ID == category.ID);

			return summaryList;
		}


	}
}
