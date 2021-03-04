using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DFM.Generic.Datetime
{
	public class TZ
	{
		public static void Init(Boolean isDev)
		{
			tz = new TZ(isDev);
		}

		private static TZ tz;

		public static IDictionary<String, String> All => tz.all;

		public static String GetTimeZone(Int32 offset) => tz.getTimeZone(offset);
		public static Boolean IsValid(String timezone) => tz.isValid(timezone);
		public static DateTime Now(String timezoneName) => tz.now(timezoneName);

		private readonly ReadOnlyCollection<Timezone> timezones;
		private readonly IDictionary<String, Timezone> timezoneDic;
		private readonly IDictionary<String, String> all;

		private TZ(Boolean isDev)
		{
			timezones = getTimezones(isDev);
			timezoneDic = timezones.ToDictionary(tz => tz.ToString(), tz => tz);
			all = timezoneDic.ToDictionary(tz => tz.Key, tz => tz.Key);
		}

		private ReadOnlyCollection<Timezone> getTimezones(Boolean isDev)
		{
			var path = Path.Combine("Datetime", "timezones.json");
			if (isDev) path = Path.Combine("bin", path);

			return File.Exists(path)
				? JsonConvert.DeserializeObject
					<ReadOnlyCollection<Timezone>>(
						File.ReadAllText(path)
					)
				: new ReadOnlyCollection<Timezone>(new List<Timezone>());
		}

		private DateTime now(String timezoneName)
		{
			if (!isValid(timezoneName))
				return DateTime.UtcNow;

			var timeZone = timezoneDic[timezoneName];

			return DateTime.UtcNow
				.AddHours(timeZone.Hour)
				.AddMinutes(timeZone.Minute);
		}

		private String getTimeZone(Int32 offset) =>
			timezones.First(o => o.Is(offset)).ToString();

		private Boolean isValid(String timeZone) =>
			all.ContainsKey(timeZone);
	}
}
