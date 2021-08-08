using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsCategoriesModel : BaseAccountModel
	{
		public ReportsCategoriesModel(Int32? id)
		{
			var dateMonth = DateFromInt.GetDateMonth(id, now);
			var dateYear = DateFromInt.GetDateYear(id, now);

			var data = report.GetCategoryReport(CurrentAccountUrl, dateYear, dateMonth);

			ChartData = data.List;

			Month = dateMonth;
			Year = dateYear;
		}

		public IList<CategoryValue> ChartData { get; }

		public IList<CategoryValue> ChartDataOut =>
			ChartData.Where(d => d.Out > 0).ToList();

		public IList<CategoryValue> ChartDataIn =>
			ChartData.Where(d => d.In > 0).ToList();

		public Int32 Month { get; }
		public Int32 Year { get; }

		public String Date =>
			String.Format(
				translator["ShortDateFormat"],
				translator.GetMonthName(Month),
				Year
			);
	}
}
