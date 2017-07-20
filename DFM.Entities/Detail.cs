using System;
using DFM.Entities.Bases;

namespace DFM.Entities
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
        public virtual FutureMove FutureMove { get; set; }


        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Description);
        }

    }
}
