using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class BaseAccountModel : BaseSiteModel
	{
		public BaseAccountModel()
		{
			CurrentAccountUrl = route["accountUrl"];
			Account = admin.GetAccount(CurrentAccountUrl);
		}

		public String CurrentAccountUrl { get; }
		public AccountInfo Account { get; }
	}
}
