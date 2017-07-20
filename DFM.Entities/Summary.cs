using System;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public class Summary : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual Double In { get; set; }
        public virtual Double Out { get; set; }
        public virtual SummaryNature Nature { get; set; }

        public virtual Category Category { get; set; }
        public virtual Month Month { get; set; }
        public virtual Year Year { get; set; }



        public override String ToString()
        {
            return String.Format("{0} - {1}",
                Category, (ISummarizable) Month ?? Year);
        }
    }
}
