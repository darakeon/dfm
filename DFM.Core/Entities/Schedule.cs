using System;
using System.Collections.Generic;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;
using DFM.Core.Helpers;

namespace DFM.Core.Entities
{
    public class Schedule : IEntity
    {
        public Schedule()
        {
            Times = 1;
        }


        public virtual Int32 ID { get; set; }

        public virtual Int32 Times { get; set; }
        public virtual DateTime Begin { get; set; }
        public virtual DateTime Next { get; set; }
        public virtual ScheduleFrequency Frequency { get; set; }

        public virtual User User { get; set; }
        public virtual IList<Move> MoveList { get; set; }
    }
}
