using System;

namespace DFM.MVC.Helpers.Views
{
	public class MonthYearUnit
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

		public static MonthYearUnit operator +(MonthYearUnit unit, Int32 months)
		{
			var date = unit.ToDate().AddMonths(months);
			return new MonthYearUnit(date);
		}

		public static Boolean operator <(MonthYearUnit unit, MonthYearUnit other)
		{
			return unit.ToDate() < other.ToDate();
		}

		public static Boolean operator >(MonthYearUnit unit, MonthYearUnit other)
		{
			return unit.ToDate() > other.ToDate();
		}

		public static Boolean operator !=(MonthYearUnit @this, MonthYearUnit other)
		{
			return @this > other || @this < other;
		}

		public static Boolean operator ==(MonthYearUnit @this, MonthYearUnit other)
		{
			return !(@this != other);
		}

		public String Label => $"{Year}-{Month.ToString("00")}";
		public String Url => $"{Year}{Month.ToString("00")}";

		public override string ToString()
		{
			return Label;
		}

		public DateTime ToDate()
		{
			return new DateTime(Year, Month, 1);
		}
	}
}