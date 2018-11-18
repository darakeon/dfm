using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class CategoriesIndexModel : BaseSiteModel
	{
		public CategoriesIndexModel()
		{
			CategoryList = admin.GetCategoryList();
		}

		public IList<Category> CategoryList;
	}
}