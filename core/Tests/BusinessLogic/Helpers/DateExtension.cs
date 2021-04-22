using System;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Tests.Helpers
{
	internal static class DateExtension
	{
		internal static void AddByFrequency(this IDate entity, String frequency, Int32 qty)
		{
			entity.SetDate(
				entity.GetDate()
					.addByFrequency(frequency, qty)
			);
		}

		private static DateTime addByFrequency(this DateTime start, String frequency, Int32 qty)
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
