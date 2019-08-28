using System;
using System.Linq;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class CategoryRepository : BaseRepository<Category>
	{
		internal Category SaveOrUpdate(Category category)
		{
			return SaveOrUpdate(category, complete, validate);
		}

		private void validate(Category category)
		{
			checkName(category);
		}

		private void checkName(Category category)
		{
			if (String.IsNullOrEmpty(category.Name))
				throw DFMCoreException.WithMessage(DfMError.CategoryNameRequired);

			if (category.Name.Length > MaxLen.Category_Name)
				throw DFMCoreException.WithMessage(DfMError.TooLargeCategoryName);

			var otherCategory = GetByName(category.Name, category.User);

			var categoryExistsForUser =
				otherCategory != null
					&& otherCategory.ID != category.ID;

			if (categoryExistsForUser)
				throw DFMCoreException.WithMessage(DfMError.CategoryAlreadyExists);
		}



		private static void complete(Category category)
		{
			if (category.ID == 0)
				category.Active = true;
		}



		internal Category GetByName(String name, User user)
		{
			var categoryList = SimpleFilter(
					a => a.Name == name
						&& a.User.ID == user.ID
				);

			if (categoryList.Count > 1)
				throw DFMCoreException.WithMessage(DfMError.DuplicatedCategoryName);

			return categoryList.SingleOrDefault();
		}



		internal void Disable(String name, User user)
		{
			alterActive(name, user, false);
		}

		internal void Enable(String name, User user)
		{
			alterActive(name, user, true);
		}

		private void alterActive(String name, User user, Boolean enable)
		{
			var category = GetByName(name, user);

			if (category == null)
				throw DFMCoreException.WithMessage(DfMError.InvalidCategory);

			if (category.Active == enable)
				throw DFMCoreException.WithMessage(
					category.Active ? DfMError.EnabledCategory : DfMError.DisabledCategory);


			category.Active = enable;
			SaveOrUpdate(category);
		}



	}
}
