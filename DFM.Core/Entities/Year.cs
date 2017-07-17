using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;

namespace DFM.Core.Entities
{
    public class Year : ISummarizable
    {
        public Year()
        {
            MonthList = new List<Month>();
            SummaryList = new List<Summary>();
        }


        public virtual Int32 ID { get; set; }

        public virtual Int32 Time { get; set; }

        public virtual Account Account { get; set; }

        public virtual IList<Month> MonthList { get; set; }
        public virtual IList<Summary> SummaryList { get; set; }


        public virtual Double CheckUp(Category category)
        {
            return MonthList.Sum(mt => mt.CheckUp(category));
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
