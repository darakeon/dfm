using System;
using System.Linq;
using DFM.BusinessLogic.Response;
using Keon.MVC.Route;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class BaseAccountModel : BaseSiteModel
	{
		public BaseAccountModel()
		{
			CurrentAccountUrl = RouteInfo.Current["accountUrl"];
			Account = admin.GetAccountByUrl(CurrentAccountUrl);
			Total = admin.GetAccountList(Account.IsOpen)
				.Single(a => a.Url == CurrentAccountUrl)
				.Total;
		}

		public String CurrentAccountUrl { get; }
		public AccountInfo Account { get; }
		public Decimal Total { get; }
	}
}
