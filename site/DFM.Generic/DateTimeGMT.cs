using System;

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
    }
}
