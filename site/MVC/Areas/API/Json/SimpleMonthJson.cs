using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Api.Json
{
	public class SimpleMonthJson
	{
		public SimpleMonthJson(YearReport.MonthItem month, Translator translator)
		{
			var monthNumber = month.Number % 100;

			Number = monthNumber;
			Name = translator.GetMonthName(monthNumber);
			Total = month.CurrentTotal;
		}

		public SimpleMonthJson(Int16 month, Translator translator)
		{
			Number = month;
			Name = translator.GetMonthName(month);
			Total = 0;
		}

		public Int32 Number { get; set; }
		public String Name { get; set; }
		public Decimal Total { get; set; }
	}
}
