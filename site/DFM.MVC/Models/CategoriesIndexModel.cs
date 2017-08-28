using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class CategoriesIndexModel : BaseLoggedModel
	{
		public CategoriesIndexModel()
		{
			CategoryList = Admin.GetCategoryList();
		}

		public IList<Category> CategoryList;
	}
}