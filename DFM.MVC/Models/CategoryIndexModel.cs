using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class CategoryIndexModel : BaseLoggedModel
    {
        public CategoryIndexModel()
        {
            CategoryList = Current.User.CategoryList
                .OrderByDescending(c => c.ID)
                .OrderByDescending(c => c.Active)
                .ToList();
        }

        public IList<Category> CategoryList;
    }
}