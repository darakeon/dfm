using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Bases;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Schedule : IEntity
    {
        public Schedule()
        {
            Times = 1;
            MoveList = new List<Move>();
            Active = true;
            Frequency = ScheduleFrequency.Monthly;
        }


        public virtual Int32 ID { get; set; }

        public virtual Int16 Times { get; set; }
        public virtual Boolean Boundless { get; set; }

        public virtual DateTime Begin { get; set; }
        public virtual DateTime Next { get; set; }

        public virtual Boolean Active { get; set; }
        public virtual ScheduleFrequency Frequency { get; set; }

        public virtual User User { get; set; }
        public virtual IList<Move> MoveList { get; set; }



        public override string ToString()
        {
            return String.Format("{0} of {1}", 
                Frequency, MoveList.FirstOrDefault());
        }

    }
}