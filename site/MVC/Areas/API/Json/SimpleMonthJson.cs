using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Json
{
	public class SimpleMonthJson
	{
		public SimpleMonthJson(YearReport.MonthItem month)
		{
			var monthNumber = month.Number % 100;

			Number = monthNumber;
			Name = Translator.GetMonthName(monthNumber);
			Total = month.Total;
		}

		public SimpleMonthJson(Int16 month)
		{
			Number = month;
			Name = Translator.GetMonthName(month);
			Total = 0;
		}

		public Int32 Number { get; set; }
		public String Name { get; set; }
		public Decimal Total { get; set; }
	}
}
