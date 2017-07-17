using System;
using System.Collections.Generic;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Move : IEntity
    {
        public Move()
        {
            DetailList = new List<Detail>();
        }


        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual MoveNature Nature { get; set; }

        public virtual Category Category { get; set; }

        public virtual Month In { get; set; }
        public virtual Month Out { get; set; }
        public virtual Schedule Schedule { get; set; }

        public virtual IList<Detail> DetailList { get; set; }


        public override String ToString()
        {
            return Description;
        }
    }
}
