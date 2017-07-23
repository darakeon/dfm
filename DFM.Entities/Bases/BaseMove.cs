using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities.Bases
{
    public class BaseMove : IEntity
    {
        public BaseMove()
        {
            DetailList = new List<Detail>();
        }

        
        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual MoveNature Nature { get; set; }

        public virtual Category Category { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual IList<Detail> DetailList { get; set; }


        public virtual Double Value()
        {
            return DetailList.Sum(d => d.Value * d.Amount);
        }

        public virtual Boolean Show()
        {
            return Date <= DateTime.Now;
        }


        public virtual User User() { throw new NotImplementedException(); }
        public virtual Account AccOut() { throw new NotImplementedException(); }
        public virtual Account AccIn() { throw new NotImplementedException(); }


        public virtual T CastToChild<T>()
            where T : BaseMove
        {
            return ConvertToChild.Convert<BaseMove, T>(this);
        }



        public override String ToString()
        {
            return Description;
        }


    }
}
