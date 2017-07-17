using System;
using System.Collections.Generic;
using DFM.Core.Entities.Bases;

namespace DFM.Core.Entities
{
    public class User : IEntity
    {
        public User()
        {
            AccountList = new List<Account>();
            CategoryList = new List<Category>();
        }



        public virtual Int32 ID { get; set; }

        public virtual String Login { get; set; }
        public virtual String Password { get; set; }
        public virtual String Email { get; set; }

        public virtual IList<Account> AccountList { get; set; }
        public virtual IList<Category> CategoryList { get; set; }


        
        public override String ToString()
        {
            return Login;
        }
    }
}
