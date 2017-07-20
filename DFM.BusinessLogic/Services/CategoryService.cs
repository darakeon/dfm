using System;
using System.Linq;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    public class CategoryService : BaseService<Category>
    {
        internal CategoryService(DataAccess father, IRepository repository) : base(father, repository) { }

        public Category SaveOrUpdate(Category category)
        {
            return SaveOrUpdate(category, complete, validate);
        }



        private static void validate(Category category)
        {
            checkName(category);
        }

        private static void checkName(Category category)
        {
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



        internal static Category SelectByName(String name, User user)
        {
            var categoryList = user.CategoryList
                .Where(c => c.Name == name)
                .ToList();

            if (categoryList.Count > 1)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedAccountName);

            return categoryList.SingleOrDefault();
        }



        public void Disable(Category category)
        {
            alterActive(category, false);
        }

        public void Enable(Category category)
        {
            alterActive(category, true);
        }

        private void alterActive(Category category, Boolean enable)
        {
            category.Active = enable;
            SaveOrUpdate(category);
        }

    }
}
