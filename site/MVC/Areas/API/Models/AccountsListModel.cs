﻿using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.Api.Models
{
	internal class AccountsListModel : BaseApiModel
	{
		public AccountsListModel()
		{
			AccountList =
				admin.GetAccountList(true)
					.OrderBy(a => a.Name)
					.ToList();
		}

		public IList<AccountListItem> AccountList { get; }
	}
}
