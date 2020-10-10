using System;
using System.Collections.Generic;

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

		public class MonthItem
		{
			public Int32 Number { get; set; }

			public Int32 CurrentInCents { get; set; }
			public Int32 CurrentOutCents { get; set; }

			public Decimal CurrentIn => CurrentInCents / 100m;
			public Decimal CurrentOut => CurrentOutCents / 100m;
			public Decimal CurrentTotal => CurrentIn - CurrentOut;
		}
	}
}
