using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class Account : IEntity
    {
        public Account()
        {
            YearList = new List<Year>();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual Double? RedLimit { get; set; }
        public virtual Double? YellowLimit { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        
        public virtual User User { get; set; }

        public virtual IList<Year> YearList { get; set; }



        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Name);
        }


        public virtual Year this[Int32 yearDate, Boolean orNew = false]
        {
            get
            {
                var year = YearList
                    .SingleOrDefault(y => y.Time == yearDate);

                return orNew && year == null
                    ? new Year() : year;
            }
        }

    }
}
