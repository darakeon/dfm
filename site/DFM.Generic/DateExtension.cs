using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic
{
	public static class DateExtension
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

		public static DateTime Now(this String timeZoneName)
		{
			if (!TimeZoneList.ContainsKey(timeZoneName))
				return DateTime.UtcNow;

			var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

			return TimeZoneInfo.ConvertTime(
				DateTime.UtcNow,
				TimeZoneInfo.Utc,
				timeZone
			);
		}

		public static IDictionary<String, String> TimeZoneList =
			TimeZoneInfo.GetSystemTimeZones()
				.ToDictionary(tz => tz.StandardName, tz => tz.DisplayName);

		public static Boolean IsTimezone(this String timezone)
		{
			return timezone == null
			       || TimeZoneList.ContainsKey(timezone);
		}
	}
}
