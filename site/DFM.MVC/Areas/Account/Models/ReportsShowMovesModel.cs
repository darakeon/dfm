using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsShowMovesModel : BaseAccountModel, ITotal
	{
		public ReportsShowMovesModel(Int32? id)
		{
			var dateMonth = DateFromInt.GetDateMonth(id, now);
			var dateYear = DateFromInt.GetDateYear(id, now);

			var month = report.GetMonthReport(CurrentAccountUrl, dateMonth, dateYear);

			MoveList = month.MoveList;
			Total = month.AccountTotal;
			
			Month = dateMonth;
			Year = dateYear;
		}

		public Decimal Total { get; }

		public IList<MoveInfo> MoveList { get; set; }

		public Int32 Month { get; set; }
		public Int32 Year { get; set; }

		public String Date =>
			String.Format(
				Translator.Dictionary["ShortDateFormat"],
				Translator.GetMonthName(Month),
				Year
			);

		public Boolean CanCheck => moveCheckingEnabled;
	}
}
