using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Repositories
{
	internal class CategoryRepository : Repo<Category>
	{
		internal Category Save(Category category)
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
				throw Error.CategoryNameRequired.Throw();

			if (category.Name.Length > MaxLen.CategoryName)
				throw Error.TooLargeCategoryName.Throw();

			var otherCategory = GetByName(category.Name, category.User);

			var categoryExistsForUser =
				otherCategory != null
					&& otherCategory.ID != category.ID;

			if (categoryExistsForUser)
				throw Error.CategoryAlreadyExists.Throw();
		}

		private static void complete(Category category)
		{
			if (category.ID == 0)
				category.Active = true;
		}

		internal Category GetByName(String name, User user)
		{
			var categoryList = Where(
					a => a.Name == name
						&& a.User.ID == user.ID
				);

			if (categoryList.Count > 1)
				throw Error.DuplicatedCategoryName.Throw();

			return categoryList.SingleOrDefault();
		}

		internal void Disable(Category category)
		{
			alterActive(category, false);
		}

		internal void Enable(Category category)
		{
			alterActive(category, true);
		}

		private void alterActive(Category category, Boolean enable)
		{
			if (category.Active == enable)
			{
				var error = category.Active
					? Error.EnabledCategory
					: Error.DisabledCategory;

				throw error.Throw();
			}

			category.Active = enable;
			Save(category);
		}

		internal IList<Category> Get(User user, Boolean? active)
		{
			var query = NewQuery()
				.Where(a => a.User.ID == user.ID);

			if (active.HasValue)
				query.Where(c => c.Active == active.Value);
			else
				query.OrderBy(c => c.Active, false);

			return query.OrderBy(a => a.Name).List;
		}
	}
}
