using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using Keon.MVC.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Models
{
	public class CategoriesUnifyModel : BaseSiteModel
	{
		public CategoriesUnifyModel()
		{
			categoryList = admin.GetCategoryList(true);
		}

		public CategoriesUnifyModel(String categoryName) : this()
		{
			CategoryToDelete = categoryName;
		}

		public String CategoryToDelete { get; set; }
		public String CategoryToKeep { get; set; }

		private readonly IList<CategoryListItem> categoryList;
		public SelectList OtherCategoriesSelectList =>
			SelectListExtension.CreateSelect(
				categoryList
					.Where(c => c.Name != CategoryToDelete)
					.ToList()
				, c => c.Name
				, c => c.Name
			);

		internal CoreError Unify()
		{
			try
			{
				admin.UnifyCategory(CategoryToKeep, CategoryToDelete);
			}
			catch (CoreError e)
			{
				return e;
			}

			return null;
		}
	}
}
