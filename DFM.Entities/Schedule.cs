using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public class Schedule : IEntity
    {
        public Schedule()
        {
            Times = 1;

            FutureMoveList = new List<FutureMove>();
            MoveList = new List<Move>();
            
            Active = true;
            Frequency = ScheduleFrequency.Monthly;
        }


        public virtual Int32 ID { get; set; }

        public virtual Int16 Times { get; set; }
        public virtual Boolean Boundless { get; set; }

        public virtual DateTime Begin { get; set; }

        public virtual Boolean Active { get; set; }
        public virtual ScheduleFrequency Frequency { get; set; }

        public virtual Boolean ShowInstallment { get; set; }

        public virtual User User { get; set; }

        public virtual IList<FutureMove> FutureMoveList { get; set; }
        public virtual IList<Move> MoveList { get; set; }



        public override string ToString()
        {
            return String.Format("[{0}] {1} of {2}", 
                ID, Frequency, FutureMoveList.FirstOrDefault());
        }


    }
}