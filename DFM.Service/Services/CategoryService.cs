using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    class CategoryService : ICategoryService
    {
        private readonly CategoryData data = new CategoryData();

        public void Disable(Category category)
        {
            data.Disable(category);
        }

        public void Enable(Category category)
        {
            data.Enable(category);
        }

        public Category SaveOrUpdate(Category category)
        {
            return (Category)data.SaveOrUpdate(category);
        }

        public Category SelectById(Int32 id)
        {
            return (Category)data.SelectById(id);
        }
    }
}