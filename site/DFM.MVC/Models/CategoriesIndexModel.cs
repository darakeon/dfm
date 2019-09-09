using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class CategoriesIndexModel : BaseSiteModel
	{
		public CategoriesIndexModel()
		{
			CategoryList = admin.GetCategoryList();
		}

		public IList<CategoryListItem> CategoryList;
	}
}
