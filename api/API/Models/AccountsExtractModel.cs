using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Generic.Datetime;

namespace DFM.API.Models
{
	internal class AccountsExtractModel : BaseApiModel
	{
		public AccountsExtractModel(String accountUrl, Int16? year, Int16? month)
		{
			var monthDate = month.GetDateMonth(now);
			var yearDate = year.GetDateYear(now);

			var extract = report.GetMonthReport(accountUrl, yearDate, monthDate);

			MoveList = extract.MoveList.Reverse()
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
