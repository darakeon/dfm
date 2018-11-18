using System;
using DFM.Entities;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsSummarizeMonthsModel : BaseAccountModel
	{
		public ReportsSummarizeMonthsModel(Int16? id)
		{
			var year = DateFromInt.GetDateYear(id, Today);

			Year = Report.GetYearReport(CurrentAccountUrl, year);
		}



		public Year Year { get; set; }

		public String Date =>
			String.Format(
				MultiLanguage.Dictionary["ShortDateFormat"],
				MultiLanguage.Dictionary["Summary"],
				Year.Time
			);
	}
}