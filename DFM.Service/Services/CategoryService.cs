using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    class CategoryService : ICategoryService
    {

        public void Disable(Category category)
        {
            CategoryData.Disable(category);
        }

        public void Enable(Category category)
        {
            CategoryData.Enable(category);
        }

        public Category SaveOrUpdate(Category category)
        {
            return (Category)CategoryData.SaveOrUpdate(category);
        }

        public Category SelectById(Int32 id)
        {
            return (Category)CategoryData.SelectById(id);
        }

    }
}