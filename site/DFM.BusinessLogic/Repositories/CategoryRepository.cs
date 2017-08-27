using System;
using System.Linq;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoryNameRequired);

            var otherCategory = GetByName(category.Name, category.User);

            var categoryExistsForUser = otherCategory != null
                                       && otherCategory.ID != category.ID;

            if (categoryExistsForUser)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoryAlreadyExists);
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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedCategoryName);

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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            if (category.Active == enable)
                throw DFMCoreException.WithMessage(
                    category.Active ? ExceptionPossibilities.EnabledCategory : ExceptionPossibilities.DisabledCategory);
            

            category.Active = enable;
            SaveOrUpdate(category);
        }



    }
}
