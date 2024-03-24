using System;
using DFM.Generic.Pages;

namespace DFM.MVC.Helpers.Views
{
	public class YearUnit : IPage
	{
		public YearUnit(Int32 year)
		{
			Year = year;
		}

		public YearUnit(DateTime date)
			: this(date.Year) { }

		public Int32 Year { get; }

		public String Label => Year.ToString();
		public String Url => Year.ToString();

		public IPage Add(Int32 qty)
		{
			var date = ToDate().AddYears(qty);
			return new YearUnit(date);
		}

		public Boolean LessThan(IPage other)
		{
			return other is YearUnit converted
					&& ToDate() < converted.ToDate();
		}

		public Boolean GreaterThan(IPage other)
		{
			return other is YearUnit converted
					&& ToDate() > converted.ToDate();
		}

		public Boolean Same(IPage other)
		{
			return other is YearUnit converted
					&& ToDate() == converted.ToDate();
		}

		public DateTime ToDate()
		{
			return new(Year, 1, 1);
		}

		public override String ToString()
		{
			return Label;
		}
	}
}
