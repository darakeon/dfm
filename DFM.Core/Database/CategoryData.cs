using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Database.Base;
using DFM.Core.Exceptions;

namespace DFM.Core.Database
{
    public class CategoryData : BaseData<Category>
    {
		private CategoryData() { }

        public static Category SaveOrUpdate(Category category)
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
            IList<Category> categoryList = Session
                .CreateCriteria(typeof(Category))
                .List<Category>()
                .Where(c => c.Name == name
                    && c.User.ID == user.ID)
                .ToList();

            if (categoryList.Count > 1)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedAccountName);

            return categoryList.SingleOrDefault();
        }



        public static void Disable(Category category)
        {
            alterActive(category, false);
        }

        public static void Enable(Category category)
        {
            alterActive(category, true);
        }

        private static void alterActive(Category category, Boolean enable)
        {
            category.Active = enable;
            SaveOrUpdate(category);
        }

    }
}
