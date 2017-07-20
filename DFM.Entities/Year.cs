using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Core.Enums;

namespace DFM.Entities
{
    public class Year : ISummarizable
    {
        public Year()
        {
            MonthList = new List<Month>();
            SummaryList = new List<Summary>();
        }


        public virtual Int32 ID { get; set; }

        public virtual Int16 Time { get; set; }

        public virtual Account Account { get; set; }

        public virtual IList<Month> MonthList { get; set; }
        public virtual IList<Summary> SummaryList { get; set; }



        public virtual Double CheckUpIn(Category category)
        {
            return MonthList.Sum(mt => mt.CheckUpIn(category));
        }

        public virtual Double CheckUpOut(Category category)
        {
            return MonthList.Sum(mt => mt.CheckUpOut(category));
        }


        
        public virtual Summary AddSummary(Category category)
        {
            var summary = new Summary
            {
                Category = category,
                Year = this,
                Nature = SummaryNature.Year,
            };

            SummaryList.Add(summary);

            return summary;
        }

        public override String ToString()
        {
            return Time.ToString();
        }

    }
}
