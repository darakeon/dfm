using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.Entities
{
    public partial class Month : ISummarizable
    {
        public Month()
        {
            init();
        }

        private void init()
        {
            SummaryList = new List<Summary>();
            InList = new List<Move>();
            OutList = new List<Move>();
        }


        public virtual Int32 ID { get; set; }

        public virtual Int16 Time { get; set; }

        public virtual Year Year { get; set; }


        public virtual IList<Summary> SummaryList { get; set; }
        public virtual IList<Move> InList { get; set; }
        public virtual IList<Move> OutList { get; set; }



        public virtual Double CheckUpIn(Category category)
        {
            var @in = sum(InList, category);

            return Math.Round(@in, 2);
        }

        public virtual Double CheckUpOut(Category category)
        {
            var @out = sum(OutList, category);

            return Math.Round(@out, 2);
        }


        private static Double sum(IEnumerable<Move> moveList, Category category)
        {
            return moveList
                .Where(m => m.Category.Is(category))
                .Sum(m => m.Value());
        }

        public virtual Summary AddSummary(Category category)
        {
            var summary = new Summary
            {
                Category = category,
                Month = this,
                Nature = SummaryNature.Month,
            };

            SummaryList.Add(summary);

            return summary;
        }


        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Time);
        }


        public virtual Summary this[String categoryName]
        {
            get
            {
                return String.IsNullOrEmpty(categoryName)
                   ? SummaryList.SingleOrDefault(m => m.Category == null)
                   : SummaryList.SingleOrDefault(m => m.Category != null && m.Category.Name == categoryName);
            }
        }

    }
}