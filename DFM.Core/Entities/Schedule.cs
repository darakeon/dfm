using System;
using System.Collections.Generic;
using DFM.Core.Entities.Base;
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
        }


        public virtual Int32 ID { get; set; }

        public virtual Int32 Times { get; set; }
        public virtual Boolean Boundless { get; set; }

        public virtual DateTime Begin { get; set; }
        public virtual DateTime Next { get; set; }

        public virtual Boolean Active { get; set; }
        public virtual ScheduleFrequency Frequency { get; set; }

        public virtual User User { get; set; }
        public virtual IList<Move> MoveList { get; set; }
    }
}