using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DFM.Generic
{
    public class DateTimeGMT
    {
        public static DateTime Now(String timeZoneName)
        {
	        try
	        {
				var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

				return TimeZoneInfo.ConvertTime(
					DateTime.UtcNow,
					TimeZoneInfo.Utc,
					timeZone
				);
			}
			catch (TimeZoneNotFoundException e)
	        {
		        Debug.Write(e.Message);
		        throw;
	        }
        }

        public static IDictionary<String, String> TimeZoneList()
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .ToDictionary(tz => tz.StandardName, tz => tz.DisplayName);
        }


    }
}
