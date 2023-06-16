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

		private const String untilMicrosecond = untilMillisecond + "fff";
		public static String UntilMicrosecond(this DateTime value)
		{
			return value.ToString(untilMicrosecond);
		}

		private const String universal = "yyyy-MM-dd";
		public static String Universal(this DateTime value)
		{
			return value.ToString(universal);
		}

		private const String universalWithTime = "yyyy-MM-dd HH:mm";
		public static String UniversalWithTime(this DateTime value)
		{
			return value.ToString(universalWithTime);
		}
	}
}
