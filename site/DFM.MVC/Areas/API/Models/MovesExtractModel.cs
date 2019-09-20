using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.API.Models
{
	internal class MovesExtractModel : BaseApiModel
	{
		public MovesExtractModel(String accountUrl, Int32 id)
		{
			var monthDate = DateFromInt.GetDateMonth(id, now);
			var yearDate = DateFromInt.GetDateYear(id, now);

			var month = report.GetMonthReport(accountUrl, monthDate, yearDate);

			MoveList = month.MoveList.Reverse()
				.Select(m => new SimpleMoveJson(m, accountUrl))
				.ToList();

			var account = 
				admin.GetAccountList(true)
					.SingleOrDefault(a => a.Url == accountUrl)
				?? admin.GetAccountList(false)
					.Single(a => a.Url == accountUrl);

			Name = account.Name;
			Total = account.Total;
			CanCheck = moveCheckingEnabled;
		}

		public IList<SimpleMoveJson> MoveList { get; private set; }
		public String Name { get; private set; }
		public Decimal Total { get; private set; }
		public Boolean CanCheck { get; private set; }

	}
}
