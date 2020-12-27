using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models.SubModels;
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

			ForeseenList = month.ForeseenList
				.Select(getSubModel)
				.ToList();

			Total = month.AccountTotal;
			TotalSign = month.AccountTotalSign;

			if (month.ForeseenTotal != 0)
			{
				Foreseen = month.ForeseenTotal;
				ForeseenSign = month.ForeseenTotalSign;
			}

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
		public AccountSign TotalSign { get; }
		public Decimal? Foreseen { get; }
		public AccountSign ForeseenSign { get; }

		public IList<MoveLineModel> MoveList { get; }
		public IList<MoveLineModel> ForeseenList { get; }

		public Int32 Month { get; }
		public Int32 Year { get; }

		public String Date =>
			String.Format(
				translator["ShortDateFormat"],
				translator.GetMonthName(Month),
				Year
			);

		public Boolean CanCheck => moveCheckingEnabled;
	}
}
