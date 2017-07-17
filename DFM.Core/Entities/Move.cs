using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Enums;
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
        public virtual DateTime Date { get; set; }
        public virtual String Description { get; set; }
        public virtual MoveNature Nature { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual Transfer Transfer { get; set; }
        public virtual Account Account { get; set; }

        public virtual IList<Detail> DetailList { get; set; }



        public virtual String Month
        {
            get { return Date.ToString("MMMM"); }
        }

        public virtual Double Value
        {
            get { return sign * DetailList.Sum(d => d.Value); }
        }

        private Int32 sign
        {
            get { return Nature == MoveNature.Out ? -1 : 1; }   
        }



        public virtual void AddDetail(Detail detail)
        {
            DetailList.Add(detail);
                detail.Move = this;
        }

        public virtual Boolean HasRealDetails()
        {
            return DetailList.Any()
                && (
                    DetailList.Count > 1
                    || DetailList[0].HasDescription()
                );
        }

        public virtual void MakeFakeDetail(Double value)
        {
            DetailList = new List<Detail>();

            var detail = new Detail {Description = Description, Value = value};

            AddDetail(detail);
        }



        public virtual Move Clone(Account otherAccount)
        {
            var move = new Move
                           {
                               Account = otherAccount,
                               Nature = Nature,
                           };
            
            move.Mirror(this);

            return move;
        }

        public virtual void Mirror(Move move)
        {
        	Date = move.Date;
            Description = move.Description;

            Category = move.Category;
            Transfer = move.Transfer;
            
            move.DetailList
                .ForEach(
                    d => DetailList.Add( 
                        d.Clone(move)
                    )
                );
        }



        public override string ToString()
        {
            return Description;
        }
    }
}
