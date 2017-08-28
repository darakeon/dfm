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

		public IList<TimeUnit> Pages { get; }

		public TimePages(TimeUnit minimum, TimeUnit maximum, TimeUnit current, Int32 rangeSize)
		{
			var first = current - (rangeSize/2);
			var last = current + (rangeSize/2);

			while (first < minimum)
			{
				first++;

				if (last < maximum)
				{ last++; }
			}

			while (last > maximum)
			{
				if (first > minimum)
				{ first--; }

				last--;
			}

			Pages = new List<TimeUnit>();

			for (var page = first; page <= last; page++)
			{
				Pages.Add(page);
			}

			MoreBack = Pages.First() - 1;
			HasMoreBack = MoreBack >= minimum;
			MoreFoward = Pages.Last() + 1;
			HasMoreFoward = MoreFoward <= maximum;
		}

	}
}