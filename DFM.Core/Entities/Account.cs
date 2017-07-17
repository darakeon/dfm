using System;
using System.Collections.Generic;
using DFM.Core.Entities.AccountDetails;
using DFM.Core.Entities.Bases;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Account : IEntity
    {
        public Account()
        {
            YearList = new List<Year>();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual AccountNature Nature { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Debt Debt { get; set; }
        public virtual Credit Credit { get; set; }
        public virtual Charge Charge { get; set; }
        
        public virtual User User { get; set; }

        public virtual IList<Year> YearList { get; set; }



        public override String ToString()
        {
            return Name;
        }

    }
}
