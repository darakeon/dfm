using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsSummarizeMonthsModel : BaseAccountModel, ITotal
	{
		public ReportsSummarizeMonthsModel(Int16? id)
		{
			var yearDate = DateFromInt.GetDateYear(id, now);
			Year = report.GetYearReport(CurrentAccountUrl, yearDate);
		}

		public Decimal Total => Year.AccountTotal;

		public YearReport Year { get; set; }

		public String Date =>
			String.Format(
				Translator.Dictionary["ShortDateFormat"],
				Translator.Dictionary["Summary"],
				Year
			);
	}
}
