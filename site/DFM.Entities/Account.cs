using System;
using System.Collections.Generic;
using DK.Generic.DB;
using DFM.Generic;

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

        public virtual Int32? RedLimitCents { get; set; }
        public virtual Int32? YellowLimitCents { get; set; }
        
        public virtual Decimal? RedLimit 
        {
            get { return RedLimitCents.ToVisual(); }
            set { RedLimitCents = value.ToCents(); }
        }

        public virtual Decimal? YellowLimit
        {
            get { return YellowLimitCents.ToVisual(); }
            set { YellowLimitCents = value.ToCents(); }
        }


        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        
        public virtual User User { get; set; }

        public virtual IList<Year> YearList { get; set; }



        

    }
}
