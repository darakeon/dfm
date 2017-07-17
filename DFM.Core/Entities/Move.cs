using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;
using DFM.Core.Helpers;
using NHibernate.Linq;

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


        //internal protected virtual String Month
        //{
        //    get { return Date.ToString("MMMM"); }
        //}


        //public virtual Double Value
        //{
        //    get { return DetailList.Sum(d => d.Value * d.Amount); }
        //}


        //internal protected virtual Boolean Show
        //{
        //    get { return Date <= DateTime.Now; }
        //}


        //[NhIgnore]
        //internal protected virtual Account AccountIn
        //{
        //    get { return In.Year.Account; }
        //}


        //[NhIgnore]
        //internal protected virtual Account AccountOut
        //{
        //    get { return Out.Year.Account; }
        //}


        //public virtual void AddDetail(Detail detail)
        //{
        //    DetailList.Add(detail);
        //    detail.Move = this;
        //}


        //public virtual Boolean HasRealDetails()
        //{
        //    return DetailList.Any()
        //        && (
        //            DetailList.Count > 1
        //            || DetailList[0].HasDescription()
        //        );
        //}


        //internal protected virtual void MakePseudoDetail(Double value)
        //{
        //    var id = (DetailList.FirstOrDefault() ?? new Detail()).ID;

        //    DetailList = new List<Detail>();

        //    var detail = new Detail { ID = id, Description = Description, Value = value };

        //    AddDetail(detail);
        //}


        //internal protected virtual Move Clone()
        //{
        //    var move = new Move
        //                   {
        //                       Description = Description,
        //                       Date = Date,
        //                       Nature = Nature,

        //                       Category = Category,

        //                       In = In,
        //                       Out = Out,
        //                   };

        //    DetailList.ForEach(d => 
        //        move.AddDetail(d.Clone(move)));

        //    return move;
        //}



        public override String ToString()
        {
            return Description;
        }
    }
}
