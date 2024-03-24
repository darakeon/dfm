using System;
using DFM.Generic.Pages;

namespace DFM.MVC.Helpers.Views
{
	public class MonthYearUnit : IPage
	{
		public MonthYearUnit(DateTime date)
		{
			Year = date.Year;
			Month = date.Month;
		}

		public Int32 Year { get; }
		public Int32 Month { get; }

		public String Label => $"{Year}-{Month:00}";
		public String Url => $"{Year}{Month:00}";

		public IPage Add(Int32 qty)
		{
			var date = ToDate().AddMonths(qty);
			return new MonthYearUnit(date);
		}

		public Boolean LessThan(IPage other)
		{
			return other is MonthYearUnit converted
					&& ToDate() < converted.ToDate();
		}

		public Boolean GreaterThan(IPage other)
		{
			return other is MonthYearUnit converted
					&& ToDate() > converted.ToDate();
		}

		public Boolean Same(IPage other)
		{
			return other is MonthYearUnit converted
					&& ToDate() == converted.ToDate();
		}

		public DateTime ToDate()
		{
			return new(Year, Month, 1);
		}

		public override String ToString()
		{
			return Label;
		}
	}
}
