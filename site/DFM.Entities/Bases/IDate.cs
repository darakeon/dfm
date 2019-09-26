using System;

namespace DFM.Entities.Bases
{
	public interface IDate
	{
		Int16 Year { get; set; }
		Int16 Month { get; set; }
		Int16 Day { get; set; }
	}

	public static class Date
	{
		public static DateTime GetDate(this IDate date)
		{
			if (date.Day == 0)
				return default;

			return new DateTime(date.Year, date.Month, date.Day);
		}

		public static void SetDate(this IDate date, DateTime value)
		{
			date.Day = (Int16)value.Day;
			date.Month = (Int16)value.Month;
			date.Year = (Int16)value.Year;
		}

		public static Int32 ToMonthYear(this IDate date)
		{
			return date.Year * 100 + date.Month;
		}
	}
}
