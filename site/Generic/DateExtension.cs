using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		private static readonly ReadOnlyCollection<TimeZoneInfo> timeZones =
			TimeZoneInfo.GetSystemTimeZones();

		public static String GetTimeZone(this Int32 offset) =>
			timeZones.First(
				tz => equal(tz.BaseUtcOffset, offset)
			).StandardName;

		private static Boolean equal(TimeSpan timeSpan, Int32 offset)
		{
			var timeSpanOffset = timeSpan.Hours * 60 + timeSpan.Minutes;
			return timeSpanOffset == offset;
		}

		public static IDictionary<String, String> TimeZoneList =
			timeZones.ToDictionary(tz => tz.StandardName, tz => tz.DisplayName);

		public static Boolean IsTimeZone(this String timeZone)
		{
			return timeZone == null
			       || TimeZoneList.ContainsKey(timeZone);
		}
	}
}
