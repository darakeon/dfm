using System;

namespace DFM.API.Helpers.Models
{
	public static class DateFromInt
	{
		public static Int16 GetDateYear(this Int16? year, DateTime today)
		{
			return year ?? (Int16)today.Year;
		}

		public static Int16 GetDateMonth(this Int16? month, DateTime today)
		{
			return month ?? (Int16)today.Month;
		}
	}
}