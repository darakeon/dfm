using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Services
{
    internal class CategoryService : BaseService<Category>
    {
        internal CategoryService(IRepository<Category> repository) : base(repository) { }

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

            var otherCategory = SelectByName(category.Name, category.User);

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



        internal Category SelectByName(String name, User user)
        {
            var categoryList = List(
                    a => a.Name == name
                         && a.User.ID == user.ID
                );

            if (categoryList.Count > 1)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedCategoryName);

            return categoryList.SingleOrDefault();
        }



        internal void Disable(Int32 id)
        {
            alterActive(id, false);
        }

        internal void Enable(Int32 id)
        {
            alterActive(id, true);
        }

        private void alterActive(Int32 id, Boolean enable)
        {
            var category = SelectById(id);

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
