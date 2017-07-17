using System;

namespace DFM.Core.Entities
{
    public class Detail
    {
        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual Double Value { get; set; }

        public virtual Move Move { get; set; }



        public virtual Detail Clone(Move move)
        {
            return new Detail
                       {
                           Description = Description,
                           Value = Value,
                           Move = move,
                       };
        }



        public virtual bool HasDescription()
        {
            return !String.IsNullOrEmpty(Description)
                && Description != Move.Description;
        }

        
        
        public override string ToString()
        {
            return Description;
        }
    }
}
