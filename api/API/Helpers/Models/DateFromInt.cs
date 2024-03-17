using System;

namespace DFM.API.Helpers.Models
{
	public class DateFromInt
	{
		internal static Int16 GetDateYear(Int16? year, DateTime today)
		{
			return year ?? (Int16)today.Year;
		}

		internal static Int16 GetDateMonth(Int16? month, DateTime today)
		{
			return month ?? (Int16)today.Month;
		}
	}
}