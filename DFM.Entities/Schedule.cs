using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public class Schedule : IEntity, IMove<Account>
    {
        public Schedule()
        {
            init();
        }

        private void init()
        {
            Times = 1;

            MoveList = new List<Move>();
            DetailList = new List<Detail>();

            Active = true;
            Frequency = ScheduleFrequency.Monthly;
        }



        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual MoveNature Nature { get; set; }
        public virtual IList<Detail> DetailList { get; set; }
        
        public virtual Boolean ShowInstallment { get; set; }

        
        public virtual DateTime Date { get; set; }
        
        public virtual Int16 LastRun { get; set; }
        public virtual Int16 Times { get; set; }

        public virtual ScheduleFrequency Frequency { get; set; }
        public virtual Boolean Boundless { get; set; }
        
        public virtual Boolean Active { get; set; }
        

        public virtual Category Category { get; set; }
        public virtual Account In { get; set; }
        public virtual Account Out { get; set; }
        public virtual User User { get; set; }


        public virtual IList<Move> MoveList { get; set; }
        


        public override string ToString()
        {
            return String.Format("[{0}] {1} x {2}", 
                ID, Frequency, Description);
        }



        //public Account AccOut()
        //{
        //    return In;
        //}

        //public Account AccIn()
        //{
        //    return Out;
        //}



    }
}