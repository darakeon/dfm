using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;
using DFM.Core.Helpers;

namespace DFM.Core.Entities
{
    public class Month : ISummarizable
    {
        public Month()
        {
            SummaryList = new List<Summary>();
            InList = new List<Move>();
            OutList = new List<Move>();
        }


        public virtual Int32 ID { get; set; }

        public virtual Int32 Time { get; set; }

        public virtual Year Year { get; set; }


        public virtual IList<Summary> SummaryList { get; set; }
        public virtual IList<Move> InList { get; set; }
        public virtual IList<Move> OutList { get; set; }



        public virtual Double Value
        {
            get { return SummaryList.Sum(s => s.SafeValue); }
        }

        [NhIgnore]
        public virtual Account Account 
        {
            get { return Year.Account; }
        }

        public virtual IList<Move> MoveList
        {
            get
            {
                var list = new List<Move>();
                
                list.AddRange(OutList);
                list.AddRange(InList);

                return list.OrderBy(m => m.ID).ToList();
            }
        }



        public virtual Double CheckUp(Category category)
        {
            var @in = InList.Where(m => m.Category == category).Sum(m => m.Value);
            var @out = OutList.Where(m => m.Category == category).Sum(m => m.Value);

            return (@in - @out) * 100 / 100;
        }

        public virtual void AjustSummaryList(Summary summary)
        {
            summary.Month = this;
            summary.Nature = SummaryNature.Month;
            summary.IsValid = false;

            SummaryList.Add(summary);
        }

        public virtual void AjustSummaryList(Category category)
        {
            if (!SummaryList.Any(s => s.Category == category))
                AjustSummaryList(new Summary { Category = category });
        }

        
        
        public virtual void AddOut(Move move)
        {
            move.Out = this;
            OutList.Add(move);
        }

        public virtual void AddIn(Move move)
        {
            move.In = this;
            InList.Add(move);
        }



        public override String ToString()
        {
            return Time.ToString();
        }

    }
}
