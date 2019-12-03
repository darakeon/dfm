using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class CategoriesCreateEditModel : BaseSiteModel
	{
		public CategoriesCreateEditModel()
		{
			Category = new CategoryInfo();
		}

		public CategoriesCreateEditModel(OperationType type) : this()
		{
			Type = type;
		}

		public CategoriesCreateEditModel(OperationType type, String categoryName) : this(type)
		{
			Category = admin.GetCategory(categoryName);
		}

		public OperationType Type { get; set; }

		public CategoryInfo Category { get; set; }

		internal CoreError CreateEdit()
		{
			try
			{
				if (Type == OperationType.Creation)
					admin.CreateCategory(Category);
				else
					admin.UpdateCategory(Category);
			}
			catch (CoreError e)
			{
				if (e.Type != Error.CategoryAlreadyExists)
					return e;
			}

			return null;
		}


	}
}
