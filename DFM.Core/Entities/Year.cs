using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Bases;
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


        public virtual Double Value
        {
            get { return SummaryList.Sum(s => s.SafeValue); }
        }



        public virtual Double CheckUp(Category category)
        {
            return MonthList.Sum(mt => mt.CheckUp(category));
        }

        public virtual void AjustSummaryList(Summary summary)
        {
            summary.Year = this;
            summary.Nature = SummaryNature.Year;
            summary.IsValid = false;

            SummaryList.Add(summary);
        }

        public virtual void AjustSummaryList(Category category)
        {
            if (!SummaryList.Any(s => s.Category == category))
                AjustSummaryList(new Summary { Category = category });
        }

        
        
        public override String ToString()
        {
            return Time.ToString();
        }

    }
}
