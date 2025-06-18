﻿using System;
using DFM.BaseWeb.Helpers.Global;
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
