using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities.Interfaces;

namespace DFM.Core.Entities
{
    public class Year : IEntity, ISummarizable
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
            get { return SummaryList.Sum(s => s.Value); }
        }



        public virtual Double CheckUp(Category category)
        {
            return MonthList.Sum(mt => mt.CheckUp(category));
        }

        public virtual void AddSummary(Summary summary)
        {
            summary.Year = this;
            SummaryList.Add(summary);
        }

        
        
        public override String ToString()
        {
            return Time.ToString();
        }
    }
}
