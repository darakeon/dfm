using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsYearModel : BaseAccountModel, ITotal
	{
		public ReportsYearModel(Int16? id)
		{
			var yearDate = DateFromInt.GetDateYear(id, now);
			Year = report.GetYearReport(CurrentAccountUrl, yearDate);
		}

		public Decimal Total => Year.AccountTotal;
		public Decimal? Foreseen => Year.AccountForeseen;

		public YearReport Year { get; set; }

		public String Date =>
			String.Format(
				translator["ShortDateFormat"],
				translator["Summary"],
				Year.Time
			);
	}
}
