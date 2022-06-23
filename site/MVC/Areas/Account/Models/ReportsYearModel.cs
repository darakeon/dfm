using System;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models.SubModels;
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

		public Decimal Current => Year.AccountTotal;
		public AccountSign CurrentSign => Year.AccountSign;
		public Decimal? Foreseen => Year.AccountForeseen;
		public AccountSign ForeseenSign => Year.AccountForeseenSign;
		public String ForeseenClass { get; set; }

		public YearReport Year { get; set; }

		public String Date =>
			String.Format(
				translator["ShortDateFormat"],
				translator["Summary"],
				Year.Time
			);
	}
}
