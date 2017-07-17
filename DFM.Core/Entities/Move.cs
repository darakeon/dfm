using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Enums;

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
            get
            {
                return Sign * DetailList.Sum(d => d.Value);
            }
        }

        private Int32 Sign
        {
            get { return Nature == MoveNature.Out ? -1 : 1; }   
        }




        public virtual Move Clone()
        {
            return Clone(Account);
        }

        public virtual Move Clone(Account otherAccount)
        {
            return new Move
            {
                Account = otherAccount,
                Category = Category,
                Date = Date,
                Nature = Nature,
                Transfer = Transfer,
                DetailList = DetailList,
            };
        }

        public virtual void AddDetail(Detail detail)
        {
            detail.Move = this;
            DetailList.Add(detail);
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


        public override string ToString()
        {
            return Description;
        }
    }
}
