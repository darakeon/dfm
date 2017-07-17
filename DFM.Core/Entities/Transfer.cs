using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Transfer : IEntity
    {
        public Transfer(Move @in, Move @out)
        {
            In = @in;
            Out = @out;
        }


        public Transfer(Move move, Account otherAccount) : this(move, move.Clone(otherAccount)) { }

        public virtual Int32 ID { get; set; }

        public virtual Move In { get; set; }
        public virtual Move Out { get; set; }


        public override string ToString()
        {
            return String.Format("{0} > {1}", Out, In);
        }
    }
}
