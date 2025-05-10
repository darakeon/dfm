using System;

namespace DFM.Generic.Datetime
{
	public static class DateFromInt
	{
		public static Int16 GetDateYear(this Int32? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;

			var dateYear = year.HasValue
				? (Int16)(year.Value / 100)
				: currentYear;

			return dateYear.forceBetween(1900, 9998);
		}

		public static Int16 GetDateMonth(this Int32? month, DateTime today)
		{
			var currentMonth = (Int16)today.Month;

			var dateMonth = month.HasValue
								? (Int16)(month.Value % 100)
								: currentMonth;

			return dateMonth.forceBetween(1, 12);
		}

		public static Int16 GetDateYear(this Int16? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;
			var dateYear = year ?? currentYear;

			return dateYear.forceBetween(1900, Int16.MaxValue);
		}

		public static Int16 GetDateMonth(this Int16? month, DateTime today)
		{
			var currentMonth = (Int16)today.Month;
			var dateMonth = month ?? currentMonth;

			return dateMonth.forceBetween(1, 12);
		}

		private static Int16 forceBetween(this Int16 number, Int16 min, Int16 max)
		{
			return number > max ? max
				: number < min ? min
				: number;
		}
	}
}
