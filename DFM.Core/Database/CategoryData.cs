using System;
using DFM.Core.Entities;
using DFM.Core.Database.Base;

namespace DFM.Core.Database
{
    public class CategoryData : BaseData<Category>
    {
		private CategoryData() { }

        public static Category SaveOrUpdate(Category category)
        {
            return SaveOrUpdate(category, null, complete);
        }

        private static void complete(Category category)
        {
            if (category.ID == 0)
                category.Active = true;
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
