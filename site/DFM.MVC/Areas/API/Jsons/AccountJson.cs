using System;
using account = DFM.Entities.Account;

namespace DFM.MVC.Areas.API.Jsons
{
	internal class AccountJson
	{
		public String Name { get; set; }
		public String Url { get; set; }

		public Decimal Total { get; set; }
		public Decimal? RedLimit { get; set; }
		public Decimal? YellowLimit { get; set; }

		public AccountJson(account account)
		{
			Name = account.Name;
			Url = account.Url;
			Total = account.Total();
			RedLimit = account.RedLimit;
			YellowLimit = account.YellowLimit;
		}



	}
}
