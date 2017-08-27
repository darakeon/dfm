using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public class Move : IMove
    {
        public Move()
        {
            init();
        }

        private void init()
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


        public virtual Month In { get; set; }
        public virtual Month Out { get; set; }
        

        
        public virtual User User()
        {
            return (In ?? Out)
                .Year.Account.User;
        }

        public virtual Account AccOut()
        {
            return getAccount(Out);
        }

        public virtual Account AccIn()
        {
            return getAccount(In);
        }

        public virtual void AddDetail(Detail detail)
        {
            DetailList.Add(detail);

            detail.Move = this;
        }

        public virtual Double Value()
        {
            return DetailList.Sum(d => d.Value * d.Amount);
        }

        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
        }


        public override String ToString()
        {
            return Description;
        }


    }
}
