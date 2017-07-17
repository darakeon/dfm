using System;
using DFM.Core.Entities.Bases;

namespace DFM.Core.Entities
{
    public class Detail : IEntity
    {
        public Detail()
        {
            Amount = 1;
        }

        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual Int16 Amount { get; set; }
        public virtual Double Value { get; set; }

        public virtual Move Move { get; set; }



        public override String ToString()
        {
            return Description;
        }
    }
}
