using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class AccountsIndexModel : BaseSiteModel
	{
		public AccountsIndexModel(Boolean open = true)
		{
			AccountList = admin.GetAccountList(open);
			HasClosed = admin.GetAccountList(false).Any();
		}

		public IList<AccountListItem> AccountList { get; set; }
		public Boolean HasClosed { get; set; }

		public Boolean AnySchedule()
		{
			return service.Robot.HasSchedule();
		}
	}
}
