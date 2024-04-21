using System;
using System.Collections.Generic;
using System.Linq;
using DFM.API.Helpers.Models;

namespace DFM.API.Models
{
	internal class MovesListsModel : BaseApiModel
	{
		public MovesListsModel()
		{
			IsUsingCategories = isUsingCategories;

			AccountList = admin.GetAccountList(true)
				.Select(a => new AccountItem(a.Name, a.Url, a.Currency?.ToString()))
				.ToList();

			if (isUsingCategories)
			{
				CategoryList = admin.GetCategoryList(true)
					.Select(a => new CategoryItem(a.Name, a.Name))
					.ToList();
			}
		}

		public bool IsUsingCategories { get; set; }
		public IList<AccountItem> AccountList { get; set; }
		public IList<CategoryItem> CategoryList { get; set; }

		internal class AccountItem : SelectItem<String, String>
		{
			public String Currency { get; }

			public AccountItem(String text, String value, String currency) : base(text, value)
			{
				Currency = currency;
			}
		}

		internal class CategoryItem : SelectItem<String, String>
		{
			public CategoryItem(string text, string value) : base(text, value)
			{
			}
		}
	}
}
