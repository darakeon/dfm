using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.Api.Helpers;

namespace DFM.MVC.Areas.Api.Models
{
	internal class MovesListsModel : BaseApiModel
	{
		public MovesListsModel()
		{
			IsUsingCategories = isUsingCategories;

			AccountList = admin.GetAccountList(true)
				.Select(a => new SelectItem<String, String>(a.Name, a.Url))
				.ToList();

			if (isUsingCategories)
			{
				CategoryList = admin.GetCategoryList(true)
					.Select(a => new SelectItem<String, String>(a.Name, a.Name))
					.ToList();
			}
		}

		public Boolean IsUsingCategories { get; set; }
		public IList<SelectItem<String, String>> AccountList { get; set; }
		public IList<SelectItem<String, String>> CategoryList { get; set; }
	}
}
