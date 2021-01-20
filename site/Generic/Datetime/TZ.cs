using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DFM.Generic.Datetime
{
	public static class TZ
	{
		public static DateTime Now(this String timeZoneName)
		{
			if (!IsValid(timeZoneName))
				return DateTime.UtcNow;

			var timeZone = timeZoneDic[timeZoneName];

			return DateTime.UtcNow
				.AddHours(timeZone.Hour)
				.AddMinutes(timeZone.Minute);
		}

		private static readonly ReadOnlyCollection<Timezone> timeZones =
			JsonConvert.DeserializeObject<ReadOnlyCollection<Timezone>>(
				File.ReadAllText(Path.Combine("Datetime", "timezones.json"))
			);

		private static readonly IDictionary<String, Timezone> timeZoneDic =
			timeZones.ToDictionary(tz => tz.ToString(), tz => tz);

		public static IDictionary<String, String> All =
			timeZoneDic.ToDictionary(tz => tz.Key, tz => tz.Key);

		public static String GetTimeZone(this Int32 offset) =>
			timeZones.First(o => o.Is(offset)).ToString();

		public static Boolean IsValid(this String timeZone) =>
			All.ContainsKey(timeZone);
	}
}
