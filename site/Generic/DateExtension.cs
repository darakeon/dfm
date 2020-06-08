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
			if (!IsTimeZone(timeZoneName))
				return DateTime.UtcNow;

			var timeZone = timeZoneDic[timeZoneName];

			return DateTime.UtcNow
				.AddHours(timeZone.Hours)
				.AddMinutes(timeZone.Minutes);
		}

		private static readonly ReadOnlyCollection<TimeSpan> timeZones =
			new ReadOnlyCollection<TimeSpan>(
				TimeZoneInfo.GetSystemTimeZones()
					.Select(tz => tz.BaseUtcOffset)
					.Distinct()
					.ToList()
			);

		private static readonly IDictionary<String, TimeSpan> timeZoneDic =
			timeZones.ToDictionary(getName, tz => tz);

		private static String getName(TimeSpan timeSpan)
		{
			var hour = timeSpan.Hours;
			var minute = timeSpan.Minutes;
			return $"UTC{hour:+00;-00; 00}:{minute:00;00;00}";
		}

		public static IDictionary<String, String> TimeZoneList =
			timeZoneDic.ToDictionary(tz => tz.Key, tz => tz.Key);

		public static String GetTimeZone(this Int32 offset) =>
			timeZones
				.First(o => equal(o, offset))
				.ToString();

		private static Boolean equal(TimeSpan timeSpan, Int32 offset)
		{
			var timeSpanOffset = timeSpan.Hours * 60 + timeSpan.Minutes;
			return timeSpanOffset == offset;
		}

		public static Boolean IsTimeZone(
			this String timeZone
		) => TimeZoneList.ContainsKey(timeZone);
	}
}
