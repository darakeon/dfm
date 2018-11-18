using System;
using DK.MVC.Route;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class BaseAccountModel : BaseSiteModel
	{
		public BaseAccountModel()
		{
			CurrentAccountUrl = RouteInfo.Current["accountUrl"];
			Account = admin.GetAccountByUrl(CurrentAccountUrl);
		}

		public String CurrentAccountUrl { get; }
		public Entities.Account Account { get; private set; }

	}

}