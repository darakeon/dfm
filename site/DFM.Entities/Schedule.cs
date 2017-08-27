using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public partial class Schedule : IMove
    {
        public Schedule()
        {
            init();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual MoveNature Nature { get; set; }
		public virtual Int32? ValueCents { get; set; }
        
        public virtual Boolean ShowInstallment { get; set; }

        

        public virtual Int16 LastRun { get; set; }
        public virtual Int16 Deleted { get; set; }
        public virtual Int16 Times { get; set; }

        public virtual ScheduleFrequency Frequency { get; set; }
        public virtual Boolean Boundless { get; set; }
        
        public virtual Boolean Active { get; set; }
        

        public virtual Category Category { get; set; }
        public virtual Account In { get; set; }
        public virtual Account Out { get; set; }
        public virtual User User { get; set; }


		public virtual IList<Detail> DetailList { get; set; }
		public virtual IList<Move> MoveList { get; set; }
        


        


    }
}