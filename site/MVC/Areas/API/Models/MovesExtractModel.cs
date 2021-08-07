using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.Api.Json;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Api.Models
{
	internal class MovesExtractModel : BaseApiModel
	{
		public MovesExtractModel(String accountUrl, Int32 id)
		{
			var monthDate = DateFromInt.GetDateMonth(id, now);
			var yearDate = DateFromInt.GetDateYear(id, now);

			var month = report.GetMonthReport(accountUrl, yearDate, monthDate);

			MoveList = month.MoveList.Reverse()
				.Select(m => new SimpleMoveJson(m, accountUrl))
				.ToList();

			var account =
				admin.GetAccountList(true)
					.SingleOrDefault(a => a.Url == accountUrl)
				?? admin.GetAccountList(false)
					.Single(a => a.Url == accountUrl);

			Title = account.Name;
			Total = account.Total;
			CanCheck = moveCheckingEnabled;
		}

		public IList<SimpleMoveJson> MoveList { get; }
		public String Title { get; }
		public Decimal Total { get; }
		public Boolean CanCheck { get; }
	}
}
