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
		public static Boolean IsValid(String timeZone) => tz.isValid(timeZone);
		public static DateTime Now(String timeZoneName) => tz.now(timeZoneName);
		public static DateTime Convert(String timeZoneName, DateTime dateTime) => tz.convert(timeZoneName, dateTime);

		private readonly ReadOnlyCollection<TimeZone> timeZones;
		private readonly IDictionary<String, TimeZone> timeZoneDic;
		private readonly IDictionary<String, String> all;

		private TZ(Boolean isDev)
		{
			timeZones = getTimeZones(isDev);
			timeZoneDic = timeZones.ToDictionary(tz => tz.ToString(), tz => tz);
			all = timeZoneDic.ToDictionary(tz => tz.Key, tz => tz.Key);
		}

		private ReadOnlyCollection<TimeZone> getTimeZones(Boolean isDev)
		{
			var path = Path.Combine("Datetime", "time-zones.json");
			if (isDev) path = Path.Combine("bin", path);

			return File.Exists(path)
				? JsonConvert.DeserializeObject
					<ReadOnlyCollection<TimeZone>>(
						File.ReadAllText(path)
					)
				: new ReadOnlyCollection<TimeZone>(new List<TimeZone>());
		}

		private DateTime now(String timeZoneName)
		{
			return convert(timeZoneName, DateTime.UtcNow);
		}

		private DateTime convert(String timeZoneName, DateTime dateTime)
		{
			if (!isValid(timeZoneName))
				return dateTime;
			
			var timeZone = timeZoneDic[timeZoneName];

			return dateTime
				.AddHours(timeZone.Hour)
				.AddMinutes(timeZone.Minute);
		}

		private String getTimeZone(Int32 offset) =>
			timeZones.First(o => o.Is(offset)).ToString();

		private Boolean isValid(String timeZone) =>
			timeZone != null && all.ContainsKey(timeZone);
	}
}
