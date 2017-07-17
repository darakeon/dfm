using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Account : IEntity
    {
        public Account()
        {
            InList = new List<Move>();
            OutList = new List<Move>();
        }



        public virtual Int32 ID { get; set; }
        
        public virtual String Name { get; set; }
        public virtual DateTime BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual AccountNature Nature { get; set; }

        public virtual User User { get; set; }

        public virtual IList<Move> InList { get; set; }
        public virtual IList<Move> OutList { get; set; }
        
        public virtual IList<Move> MoveList { 
            get
            {
                var list = InList.ToList();
                list.AddRange(OutList);
                return list;
            }
        }



        public virtual Double MovesSum
        {
            get { return MoveList.Sum(m => m.Value); }
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
