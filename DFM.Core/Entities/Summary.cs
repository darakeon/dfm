using System;
using DFM.Core.Entities.Interfaces;

namespace DFM.Core.Entities
{
    public class Summary : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual Double Value { get; set; }
        
        public virtual Category Category { get; set; }

        public virtual Month Month { get; set; }
        public virtual Year Year { get; set; }



        public override String ToString()
        {
            return String.Format("{0} - {1}",
                (ISummarizable) Month ?? Year, Category);
        }
    }
}
