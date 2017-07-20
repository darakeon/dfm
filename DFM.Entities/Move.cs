using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Core.Enums;

namespace DFM.Entities
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



        public Double Value()
        {
            return DetailList.Sum(d => d.Value * d.Amount);
        }


        public Boolean Show()
        {
            return Date <= DateTime.Now;
        }



        public override String ToString()
        {
            return Description;
        }
    }
}
