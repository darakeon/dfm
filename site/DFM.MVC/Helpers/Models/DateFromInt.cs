using System;
using DFM.MVC.Helpers.Extensions;

namespace DFM.MVC.Helpers.Models
{
	public class DateFromInt
	{
		internal static Int16 GetDateYear(Int32? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;

			var dateYear = year.HasValue
				? (Int16)(year.Value / 100)
				: currentYear;

			return dateYear.ForceBetween(1900, currentYear);
		}

		internal static Int16 GetDateMonth(Int32? month, DateTime today)
		{
			var currentMonth = (Int16)today.Month;

			var dateMonth = month.HasValue
								? (Int16)(month.Value % 100)
								: currentMonth;

			return dateMonth.ForceBetween(1, 12);
		}

		internal static Int16 GetDateYear(Int16? year, DateTime today)
		{
			var currentYear = (Int16)today.Year;
			var dateYear = year ?? currentYear;

			return dateYear.ForceBetween(1900, currentYear);
		}


	}
}