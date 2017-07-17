using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Entities.Extensions;
using DFM.Core.Enums;

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


        public virtual Double CheckUp(Category category)
        {
            var @in = sum(InList, category);
            var @out = sum(OutList, category);

            return Math.Round(@in - @out, 2);
        }

        private static Double sum(IEnumerable<Move> moveList, Category category)
        {
            return moveList
                .Where(m => m.Show() && m.Category == category)
                .Sum(m => m.Value());
        }

        public virtual void AjustSummaryList(Summary summary)
        {
            summary.Month = this;
            summary.Nature = SummaryNature.Month;
            summary.IsValid = false;

            SummaryList.Add(summary);
        }


        public override String ToString()
        {
            return Time.ToString();
        }

    }
}