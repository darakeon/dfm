using System;

namespace DFM.MVC.Helpers.Views
{
	public class YearUnit
	{
		public YearUnit(Int32 year)
		{
			Year = year;
		}

		public YearUnit(DateTime date)
			: this(date.Year) { }

		public Int32 Year { get; }

		public static YearUnit operator +(YearUnit unit, Int32 years)
		{
			var date = unit.ToDate().AddYears(years);
			return new YearUnit(date);
		}

		public static Boolean operator <(YearUnit unit, YearUnit other)
		{
			return unit.ToDate() < other.ToDate();
		}

		public static Boolean operator >(YearUnit unit, YearUnit other)
		{
			return unit.ToDate() > other.ToDate();
		}

		public static Boolean operator !=(YearUnit @this, YearUnit other)
		{
			return @this > other || @this < other;
		}

		public static Boolean operator ==(YearUnit @this, YearUnit other)
		{
			return !(@this != other);
		}

		public String Label => $"{Year}";
		public String Url => $"{Year}";

		public override string ToString()
		{
			return Label;
		}

		public DateTime ToDate()
		{
			return new DateTime(Year, 1, 1);
		}
	}
}