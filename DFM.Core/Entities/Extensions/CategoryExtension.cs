using System;

namespace DFM.Core.Entities.Extensions
{
    public static class CategoryExtension
    {
        public static Boolean AuthorizeCRUD(this Category category, User user)
        {
            return category.User == user;
        }

    }
}
