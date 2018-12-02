using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class CategoriesCreateEditModel : BaseSiteModel
	{
		public CategoriesCreateEditModel()
		{
			Category = new Category();
		}

		public CategoriesCreateEditModel(OperationType type) : this()
		{
			Type = type;
		}

		public CategoriesCreateEditModel(OperationType type, String categoryName) : this(type)
		{
			Category = admin.GetCategoryByName(categoryName);
		}



		public OperationType Type { get; set; }

		public Category Category { get; set; }

		private String name;

		[Required(ErrorMessage = "*")]
		public String Name
		{
			get
			{
				switch (Type)
				{
					case OperationType.Creation:
						return Category.Name;
					case OperationType.Edition:
						return name ?? Category.Name;
					default:
						throw new NotImplementedException();
				}
			}
			set
			{
				name = value;

				if (Type == OperationType.Creation)
					Category.Name = value;
			}
		}



		internal DFMCoreException CreateEdit()
		{
			try
			{
				if (Type == OperationType.Creation)
					admin.CreateCategory(Category);
				else
					admin.UpdateCategory(Category, Name);
			}
			catch (DFMCoreException e)
			{
				if (e.Type != ExceptionPossibilities.CategoryAlreadyExists)
					return e;
			}

			return null;
		}


	}
}