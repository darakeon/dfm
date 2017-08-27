using System;

namespace DFM.Entities
{
    public partial class Category
    {
        public virtual Boolean AuthorizeCRUD(User user)
        {
            return User == user;
        }
    }
}
