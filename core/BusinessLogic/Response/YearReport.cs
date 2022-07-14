using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class YearReport
	{
		public YearReport(Account account, Decimal total, Decimal foreseen, Int32 time, IList<MonthItem> months)
		{
			AccountTotal = total;
			if (account.User.Settings.UseAccountsSigns)
				AccountSign = account.GetSign(total);

			AccountForeseen = total + foreseen;
			if (account.User.Settings.UseAccountsSigns)
				AccountForeseenSign = account.GetSign(total + foreseen);

			Time = time;
			MonthList = months;
		}

		public Decimal AccountTotal { get; }
		public AccountSign AccountSign { get; }
		public Decimal AccountForeseen { get; }
		public AccountSign AccountForeseenSign { get; }

		public Int32 Time { get; }
		public IList<MonthItem> MonthList { get; }

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

			public Decimal ForeseenInCents { get; set; }
			public Decimal ForeseenOutCents { get; set; }

			public Decimal ForeseenIn => ForeseenInCents / 100m;
			public Decimal ForeseenOut => ForeseenOutCents / 100m;
			public Decimal ForeseenTotal => ForeseenIn - ForeseenOut;
		}
	}
}
