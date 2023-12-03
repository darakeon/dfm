using System;

namespace DFM.API.Helpers.Models
{
	public class DateFromInt
	{
		internal static Int16 GetDateYear(Int32? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;

			var dateYear = year.HasValue
				? (Int16)(year.Value / 100)
				: currentYear;

			return forceBetween(dateYear, 1900, 9998);
		}

		internal static Int16 GetDateMonth(Int32? month, DateTime today)
		{
			var currentMonth = (Int16)today.Month;

			var dateMonth = month.HasValue
								? (Int16)(month.Value % 100)
								: currentMonth;

			return forceBetween(dateMonth, 1, 12);
		}

		internal static Int16 GetDateYear(Int16? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;
			var dateYear = year ?? currentYear;

			return forceBetween(dateYear, 1900, Int16.MaxValue);
		}

		private static Int16 forceBetween(Int16 number, Int16 min, Int16 max)
		{
			return number > max ? max
				: number < min ? min
				: number;
		}

	}
}