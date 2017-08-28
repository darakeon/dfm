using System;

namespace DFM.Generic
{
	public static class DateTimeExtension
	{
		public static String ToShortDateString(this DateTime? dateTime)
		{
			return dateTime?.ToShortDateString();
		}

		public static Int32? ToMonthYear(this DateTime? dateTime)
		{
			return dateTime?.ToMonthYear();
		}

		public static Int32 ToMonthYear(this DateTime dateTime)
		{
			return dateTime.Year * 100 + dateTime.Month;
		}

	}
}
