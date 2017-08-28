using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.API.Jsons;

namespace DFM.MVC.Areas.API.Models
{
	internal class AccountsListModel : BaseApiModel
	{
		public AccountsListModel()
		{
			AccountList = 
				Current.User.VisibleAccountList()
					.OrderBy(a => a.Name)
					.Select(a => new AccountJson(a))
					.ToList();
		}

		public IList<AccountJson> AccountList { get; private set; }

	}
}