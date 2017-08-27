using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic
{
    public class DateTimeGMT
    {
        public static DateTime Now(String timeZoneName)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

            return TimeZoneInfo.ConvertTime(
                DateTime.UtcNow,
                TimeZoneInfo.Utc,
                timeZone
            );
        }

        public static IDictionary<String, String> TimeZoneList()
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .ToDictionary(tz => tz.StandardName, tz => tz.DisplayName);
        }


    }
}
