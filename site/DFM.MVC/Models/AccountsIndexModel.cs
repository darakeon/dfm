using System;
using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class AccountsIndexModel : BaseLoggedModel
	{
		public AccountsIndexModel(Boolean open = true)
		{
			AccountList = Admin.GetAccountList(open);
		}

		public IList<Account> AccountList { get; set; }
	}
}