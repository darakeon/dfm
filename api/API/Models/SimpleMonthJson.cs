using DFM.API.Helpers.Global;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models
{
	public class SimpleMonthJson
	{
		public SimpleMonthJson(YearReport.MonthItem month, Translator translator)
		{
			var number = month.Number % 100;

			Number = number;
			Name = translator.GetMonthName(number);
			Total = month.CurrentTotal;
		}

		public SimpleMonthJson(short month, Translator translator)
		{
			Number = month;
			Name = translator.GetMonthName(month);
			Total = 0;
		}

		public int Number { get; set; }
		public string Name { get; set; }
		public decimal Total { get; set; }
	}
}
