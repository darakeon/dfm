using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class YearReport
	{
		public YearReport(Decimal total, Int32 time, IList<Summary> summaryList)
		{
			AccountTotal = total;

			Time = time;

			// TODO: use summarize
			MonthList = summaryList
				.GroupBy(s => s.Time)
				.Select(g =>
					new MonthItem(
						g.Key,
						g.Sum(s => s.In),
						g.Sum(s => s.Out)
					)
				)
				.ToList();
		}

		public Decimal AccountTotal { get; set; }
		public Int32 Time { get; set; }
		public IList<MonthItem> MonthList { get; set; }

		public class MonthItem
		{
			public MonthItem(Int32 number, Decimal @in, Decimal @out)
			{
				Number = number;
				In = @in;
				Out = @out;
				Total = In - Out;
			}

			public Int32 Number { get; }
			public Decimal In { get; }
			public Decimal Out { get; }
			public Decimal Total { get; }
		}
	}
}
