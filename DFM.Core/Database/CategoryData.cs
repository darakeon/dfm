using System;
using DFM.Core.Database.Base;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class CategoryData : BaseData<Category>
    {
        public CategoryData()
        {
            Complete += complete;
        }


        private static void complete(Category category)
        {
            if (category.ID == 0)
                category.Active = true;
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
