using System;
using DK.MVC.Route;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class BaseAccountModel : BaseLoggedModel
	{
		public BaseAccountModel()
		{
			var routeInfo = new RouteInfo();

			CurrentAccountUrl = routeInfo.RouteData.Values["accounturl"].ToString();

			Account = Admin.GetAccountByUrl(CurrentAccountUrl);
		}

		public String CurrentAccountUrl { get; }
		public Entities.Account Account { get; private set; }

	}

}