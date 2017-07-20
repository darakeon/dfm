using System;
using DFM.Entities;

namespace DFM.Extensions.Entities
{
    public static class CategoryExtension
    {
        public static Boolean AuthorizeCRUD(this Category category, User user)
        {
            return category.User == user;
        }

    }
}
