using System;
using DFM.Generic.Pages;

namespace DFM.MVC.Helpers.Views
{
	public class MonthYearUnit : IPage
	{
		public MonthYearUnit(Int32 year, Int32 month)
		{
			Year = year;
			Month = month;
		}

		public MonthYearUnit(DateTime date)
			: this(date.Year, date.Month)
		{ }

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
			return new DateTime(Year, Month, 1);
		}

		public override string ToString()
		{
			return Label;
		}
	}
}
