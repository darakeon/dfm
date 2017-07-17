using System;
using System.Collections.Generic;
using DFM.Core.Entities.Base;

namespace DFM.Core.Entities
{
    public class User : IEntity
    {
        public User()
        {
            AccountList = new List<Account>();
            CategoryList = new List<Category>();
            ScheduleList = new List<Schedule>();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Password { get; set; }
        public virtual String Email { get; set; }
        public virtual String Language { get; set; }

        public virtual IList<Account> AccountList { get; set; }
        public virtual IList<Category> CategoryList { get; set; }
        public virtual IList<Schedule> ScheduleList { get; set; }


        public override String ToString()
        {
            return Email;
        }
    }
}
