using System;
using DFM.Service.Entities;

namespace DFM.Service
{
    public interface ICategoryService
    {
        void Disable(Category category);
        void Enable(Category category);
        Category SaveOrUpdate(Category entity);
        Category SelectById(Int32 id);
    }
}
