using System;
using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class AccountsIndexModel : BaseSiteModel
	{
		public AccountsIndexModel(Boolean open = true)
		{
			AccountList = admin.GetAccountList(open);
		}

		public IList<Account> AccountList { get; set; }
	}
}