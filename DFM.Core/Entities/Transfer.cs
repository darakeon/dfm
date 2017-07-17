using System;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Transfer : IEntity
    {
        public Transfer() { }

        public Transfer(Move outMove, Move inMove) : this()
        {
            outMove.Transfer = this;
            outMove.Nature = MoveNature.Out;
            Out = outMove;

            inMove.Transfer = this;
            inMove.Nature = MoveNature.In;
            In = inMove;
        }

        public Transfer(Move move, Account otherAccount)
            : this(move, move.Clone(otherAccount)) { }



        public virtual Int32 ID { get; set; }

        public virtual Move In { get; set; }
        public virtual Move Out { get; set; }


        public override string ToString()
        {
            return String.Format("{0} / {1}", In, Out);
        }

    }
}
