using System;
using DFM.Core.Entities.Base;

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
        public virtual Int32 Amount { get; set; }
        public virtual Double Value { get; set; }

        public virtual Move Move { get; set; }



        public virtual Detail Clone(Move move)
        {
            return new Detail
                       {
                           Description = Description,
                           Amount = Amount,
                           Value = Value,
                           Move = move,
                       };
        }



        public virtual Boolean HasDescription()
        {
            return !String.IsNullOrEmpty(Description)
                && Description != Move.Description;
        }

        
        
        public override String ToString()
        {
            return Description;
        }
    }
}
