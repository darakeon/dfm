using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.BusinessLogic.Response
{
	public class YearReport
	{
		public YearReport(Decimal total, Int32 time, IList<MonthItem> months)
		{
			AccountTotal = total;
			Time = time;
			MonthList = months;
		}

		public Decimal AccountTotal { get; set; }
		public Int32 Time { get; set; }
		public IList<MonthItem> MonthList { get; set; }

		public Decimal CurrentIn => MonthList.Sum(m => m.CurrentIn);
		public Decimal CurrentOut => MonthList.Sum(m => m.CurrentOut);
		public Decimal CurrentTotal => MonthList.Sum(m => m.CurrentTotal);

		public Decimal ForeseenIn => MonthList.Sum(m => m.ForeseenIn);
		public Decimal ForeseenOut => MonthList.Sum(m => m.ForeseenOut);
		public Decimal ForeseenTotal => MonthList.Sum(m => m.ForeseenTotal);

		public class MonthItem
		{
			public Int32 Number { get; set; }

			public Int32 CurrentInCents { get; set; }
			public Int32 CurrentOutCents { get; set; }

			public Decimal CurrentIn => CurrentInCents / 100m;
			public Decimal CurrentOut => CurrentOutCents / 100m;
			public Decimal CurrentTotal => CurrentIn - CurrentOut;

			public Decimal ForeseenIn { get; set; }
			public Decimal ForeseenOut { get; set; }
			public Decimal ForeseenTotal => ForeseenIn - ForeseenOut;
		}
	}
}
