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
			return date.Day == 0
				? default
				: createDate(date);
		}

		private static DateTime createDate(IDate date)
		{
			var nextMonth = date.Month == 12
				? new DateTime(date.Year + 1, 1, 1)
				: new DateTime(date.Year, date.Month + 1, 1);

			var lastDay = nextMonth.AddDays(-1);

			if (date.Day > lastDay.Day)
				date.Day = (Int16) lastDay.Day;

			return new DateTime(date.Year, date.Month, date.Day);
		}

		public static DateParent SetDate<DateParent>(this DateParent date, DateTime value)
			where DateParent : IDate
		{
			date.Day = (Int16)value.Day;
			date.Month = (Int16)value.Month;
			date.Year = (Int16)value.Year;
			return date;
		}

		public static void SetDate(this IDate date, String value)
		{
			date.SetDate(DateTime.Parse(value));
		}

		public static Int32 ToMonthYear(this IDate date)
		{
			return date.Year * 100 + date.Month;
		}
	}
}
