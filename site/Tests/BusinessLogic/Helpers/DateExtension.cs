using System;

namespace DFM.BusinessLogic.Tests.Helpers
{
	internal static class DateExtension
	{
		internal static DateTime AddByFrequency(this DateTime start, String frequency, Int32 qty)
		{
			switch (frequency)
			{
				case "day":
					return start.AddDays(qty);
				case "month":
					return start.AddMonths(qty);
				case "year":
					return start.AddYears(qty);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
