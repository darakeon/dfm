using System;
using System.Collections.Generic;
using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class User : IEntity
    {
        public User()
        {
            init();
        }

        private void init()
        {
            AccountList = new List<Account>();
            CategoryList = new List<Category>();
            ScheduleList = new List<Schedule>();
            SecurityList = new List<Security>();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Password { get; set; }
        public virtual String Email { get; set; }
        public virtual DateTime Creation { get; set; }

        public virtual String Language { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual Boolean SendMoveEmail { get; set; }

        public virtual IList<Account> AccountList { get; set; }
        public virtual IList<Category> CategoryList { get; set; }
        public virtual IList<Schedule> ScheduleList { get; set; }

        public virtual IList<Security> SecurityList { get; set; }


        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Email);
        }

    }
}
