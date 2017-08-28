using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.MVC.Helpers.Models
{
	public class TimePages
	{
		public TimeUnit MoreBack { get; }
		public TimeUnit MoreFoward { get; }
		public Boolean HasMoreBack { get; }
		public Boolean HasMoreFoward { get; }

		public TimeUnit Current { get; }
		public IList<TimeUnit> Pages { get; }

		public TimePages(DateTime beginDate, DateTime endDate, Int32 year, Int32 month, Int32 limit)
		{
			Current = new TimeUnit(year, month);

			var first = Current - (limit / 2);
			var last = Current + (limit / 2);

			while (first < beginDate)
			{
				first++;

				if (last < endDate)
				{ last++; }
			}

			while (last > endDate)
			{
				if (first > beginDate)
				{ first--; }

				last--;
			}

			Pages = new List<TimeUnit>();

			for (var page = first; page <= last.ToDate(); page++)
			{
				Pages.Add(page);
			}

			MoreBack = Pages.First() - 1;
			HasMoreBack = MoreBack >= beginDate;
			MoreFoward = Pages.Last() + 1;
			HasMoreFoward = MoreFoward <= endDate;
		}


	}
}