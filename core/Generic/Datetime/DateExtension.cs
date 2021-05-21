using System;

namespace DFM.Generic.Datetime
{
	public static class DateExtension
	{
		public static String ToShortDateString(this DateTime? value)
		{
			return value?.ToShortDateString();
		}

		public static Int32? ToMonthYear(this DateTime? value)
		{
			return value?.ToMonthYear();
		}

		public static Int32 ToMonthYear(this DateTime value)
		{
			return value.Year * 100 + value.Month;
		}

		private const String untilSecond = "yyyyMMddHHmmss";
		public static String UntilSecond(this DateTime value)
		{
			return value.ToString(untilSecond);
		}

		private const String untilMillisecond = untilSecond + "fff";
		public static String UntilMillisecond(this DateTime value)
		{
			return value.ToString(untilMillisecond);
		}

		private const String untilNanosecond = untilMillisecond + "fff";
		public static String UntilNanosecond(this DateTime value)
		{
			return value.ToString(untilNanosecond);
		}

		private const String universal = "yyyy-MM-dd";
		public static String Universal(this DateTime value)
		{
			return value.ToString(universal);
		}
	}
}
