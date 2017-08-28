using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class CategoriesIndexModel : BaseLoggedModel
	{
		public CategoriesIndexModel()
		{
			CategoryList =
				Current.User.CategoryList
					.OrderBy(c => c.Name)
					.ThenByDescending(c => c.Active)
					.ToList();
		}

		public IList<Category> CategoryList;
	}
}