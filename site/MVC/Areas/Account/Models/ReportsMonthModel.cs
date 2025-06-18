﻿using System;
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
			var dateMonth = id.GetDateMonth(now);
			var dateYear = id.GetDateYear(now);

			var month = report.GetMonthReport(CurrentAccountUrl, dateYear, dateMonth);

			var hasForeseen = month.ForeseenList.Any();

			MoveList = month.MoveList
				.Select(getSubModel)
				.ToList();

			ForeseenList = month.ForeseenList
				.Select(getSubModel)
				.ToList();

			Current = month.AccountTotal;
			CurrentSign = month.AccountTotalSign;

			var showForeseen =
				month.ForeseenTotal != month.AccountTotal ||
				month.ForeseenTotalSign != month.AccountTotalSign;

			if (showForeseen)
			{
				Foreseen = month.ForeseenTotal;
				ForeseenSign = month.ForeseenTotalSign;
			}

			Month = dateMonth;
			Year = dateYear;

			AccountHasMoves = month.AccountHasMoves;
		}

		private MoveLineModel getSubModel(MoveInfo move)
		{
			return new(
				move,
				isUsingCategories,
				CurrentAccountUrl,
				language,
				CanCheck
			);
		}

		public Decimal Current { get; }
		public AccountSign CurrentSign { get; }
		public Decimal? Foreseen { get; }
		public AccountSign ForeseenSign { get; }
		public String ForeseenClass { get; set; }

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
		public Boolean AccountHasMoves { get; }
	}
}
