using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class CategoryListItem
	{
		private CategoryListItem(Category category)
		{
			Name = category.Name;
			Active = category.Active;
		}

		internal static CategoryListItem Convert(Category category)
			=> new(category);

		public String Name { get; }
		public Boolean Active { get; }
	}
}
