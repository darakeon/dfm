using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Base;
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

        public virtual User User { get; set; }

        public virtual IList<Year> YearList { get; set; }


        public virtual Double MovesSum
        {
            get { return YearList.Sum(m => m.Value); }
        }

        public virtual Boolean Open
        {
            get { return EndDate == null; }
        }

        public virtual Boolean HasMoves
        {
            get
            {
                return YearList.Any(
                        y => y.MonthList.Any(
                                m => m.MoveList.Any()
                            )
                    );
            }
        }



        public override String ToString()
        {
            return Name;
        }
    }
}
