using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class ReportsCategoriesModel : BaseAccountModel
	{
		public ReportsCategoriesModel(Int32? id)
		{
			if (id is < 10000)
			{
				Year = (Int16) id.Value;
			}
			else
			{
				Month = DateFromInt.GetDateMonth(id, now);
				Year = DateFromInt.GetDateYear(id, now);
			}

			var data = report.GetCategoryReport(CurrentAccountUrl, Year, Month);

			SummaryNature = data.Nature;
			ChartData = data.List;
		}

		public SummaryNature SummaryNature { get; }
		public IList<CategoryValue> ChartData { get; }

		public IList<CategoryValue> ChartDataOut =>
			ChartData.Where(d => d.Out > 0).ToList();

		public IList<CategoryValue> ChartDataIn =>
			ChartData.Where(d => d.In > 0).ToList();

		public Int16? Month { get; }
		public Int16 Year { get; }

		public String Date =>
			Month.HasValue
				? String.Format(
					translator["ShortDateFormat"],
					translator.GetMonthName(Month.Value),
					Year
				)
				: Year.ToString();
	}
}
