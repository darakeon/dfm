using System;

namespace DFM.MVC.Areas.Api.Json
{
	public class DateJson : IComparable<DateJson>
	{
		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public int CompareTo(DateJson obj)
		{
			var year = Year.CompareTo(obj.Year);

			if (year != 0)
				return year;

			var month = Month.CompareTo(obj.Month);

			if (month != 0)
				return month;

			return Day.CompareTo(obj.Day);
		}

		public override string ToString()
		{
			var year = Year.ToString("0000");
			var month = Month.ToString("00");
			var day = Day.ToString("00");

			return $"{year}-{month}-{day}";
		}
	}
}
