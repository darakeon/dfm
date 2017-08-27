using System;
using System.Collections.Generic;
using Ak.Generic.DB;

namespace DFM.Entities
{
    public partial class Account : IEntity
    {
        public Account()
        {
            init();
        }



        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual String Url { get; set; }

        public virtual Double? RedLimit { get; set; }
        public virtual Double? YellowLimit { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        
        public virtual User User { get; set; }

        public virtual IList<Year> YearList { get; set; }



        

    }
}
