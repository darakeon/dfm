using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class CategoryInfo
	{
		public CategoryInfo() { }

		private CategoryInfo(Category category)
		{
			Name = category.Name;
			OriginalName = category.Name;
		}

		internal static CategoryInfo Convert(Category category)
			=> new(category);

		public void Update(Category category)
		{
			category.Name = Name;
		}

		public String OriginalName { get; set; }

		public String Name { get; set; }
	}
}
