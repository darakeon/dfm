using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class CategoriesCreateEditModel : BaseSiteModel
	{
		public CategoriesCreateEditModel()
		{
			Type = OperationType.Creation;
			Category = new CategoryInfo();
		}

		public CategoriesCreateEditModel(String categoryName) : this()
		{
			Type = OperationType.Edition;
			Category = admin.GetCategory(categoryName);
		}

		public OperationType Type { get; set; }

		public CategoryInfo Category { get; set; }

		internal List<String> CreateEdit()
		{
			var errors = new List<String>();

			try
			{
				if (Type == OperationType.Creation)
					admin.CreateCategory(Category);
				else
					admin.UpdateCategory(Category);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
