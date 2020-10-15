using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsMonthModel : BaseAccountModel, ITotal
	{
		public ReportsMonthModel(Int32? id)
		{
			var dateMonth = DateFromInt.GetDateMonth(id, now);
			var dateYear = DateFromInt.GetDateYear(id, now);

			var month = report.GetMonthReport(CurrentAccountUrl, dateMonth, dateYear);

			MoveList = month.MoveList
				.Select(getSubModel)
				.ToList();

			Total = month.AccountTotal;
			
			Month = dateMonth;
			Year = dateYear;
		}

		private MoveLineModel getSubModel(MoveInfo move)
		{
			return new MoveLineModel(
				move,
				isUsingCategories,
				CurrentAccountUrl,
				language,
				CanCheck
			);
		}

		public Decimal Total { get; }

		public IList<MoveLineModel> MoveList { get; set; }

		public Int32 Month { get; set; }
		public Int32 Year { get; set; }

		public String Date =>
			String.Format(
				translator["ShortDateFormat"],
				translator.GetMonthName(Month),
				Year
			);

		public Boolean CanCheck => moveCheckingEnabled;
	}
}
