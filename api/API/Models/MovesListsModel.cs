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
				.Select(a => new SelectItem<string, string>(a.Name, a.Url))
				.ToList();

			if (isUsingCategories)
			{
				CategoryList = admin.GetCategoryList(true)
					.Select(a => new SelectItem<string, string>(a.Name, a.Name))
					.ToList();
			}
		}

		public bool IsUsingCategories { get; set; }
		public IList<SelectItem<string, string>> AccountList { get; set; }
		public IList<SelectItem<string, string>> CategoryList { get; set; }
	}
}
