﻿using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.Api.Json;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Api.Models
{
	internal class MovesSummaryModel : BaseApiModel
	{
		public MovesSummaryModel(String accountUrl, Int16 id)
		{
			var yearDate = DateFromInt.GetDateYear(id, now);

			MonthList =
				report.GetYearReport(accountUrl, yearDate)
					.MonthList
					.Select(m => new SimpleMonthJson(m, translator))
					.ToList();

			for (Int16 ym = 1; ym < 13; ym++)
			{
				if (MonthList.All(m => m.Number != ym))
				{
					MonthList.Add(new SimpleMonthJson(ym, translator));
				}
			}

			MonthList = MonthList
				.OrderByDescending(m => m.Number)
				.ToList();

			var account =
				admin.GetAccountList(true)
					.SingleOrDefault(a => a.Url == accountUrl)
				?? admin.GetAccountList(false)
					.Single(a => a.Url == accountUrl);

			Title = account.Name;
			Total = account.Total;
		}

		public IList<SimpleMonthJson> MonthList { get; }
		public String Title { get; }
		public Decimal Total { get; }
	}
}
