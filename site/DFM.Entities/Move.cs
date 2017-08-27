using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public partial class Move : IMove
    {
        public Move()
        {
            init();
        }

        
        
        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual DateTime Date { get; set; }
		public virtual MoveNature Nature { get; set; }
		public virtual Int32? ValueCents { get; set; }

        public virtual Category Category { get; set; }
        public virtual Schedule Schedule { get; set; }

        public virtual IList<Detail> DetailList { get; set; }


        public virtual Month In { get; set; }
        public virtual Month Out { get; set; }



        


    }
}
