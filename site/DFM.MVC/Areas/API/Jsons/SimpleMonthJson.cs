using System;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Jsons
{
	public class SimpleMonthJson
	{
		public SimpleMonthJson(Month month)
		{
			Number = month.Time;
			Name = MultiLanguage.GetMonthName(month.Time);
			Total = month.Total();
		}

		public SimpleMonthJson(Int16 month)
		{
			Number = month;
			Name = MultiLanguage.GetMonthName(month);
			Total = 0;
		}

		public Int16 Number { get; set; }
		public String Name { get; set; }
		public Decimal Total { get; set; }

	}
}