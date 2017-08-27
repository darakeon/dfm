using System;
using Ak.Generic.DB;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public class Summary : IEntity
    {
        public Summary()
        {
            init();
        }

        private void init()
        {
            Broken = true;
        }

        public Summary(Month month, Category category)
            : this()
        {
            init(month, category);
        }

        private void init(Month month, Category category)
        {
            Month = month;
            Category = category;
            Nature = SummaryNature.Month;
        }

        public Summary(Year year, Category category)
        {
            init(year, category);
        }

        private void init(Year year, Category category)
        {
            Year = year;
            Category = category;
            Nature = SummaryNature.Year;
        }



        public virtual Int32 ID { get; set; }

        public virtual Double In { get; set; }
        public virtual Double Out { get; set; }
        public virtual SummaryNature Nature { get; set; }

        public virtual Boolean Broken { get; set; }

        public virtual Category Category { get; set; }
        public virtual Month Month { get; set; }
        public virtual Year Year { get; set; }



        public override String ToString()
        {
            return String.Format("[{0}] {1}",
                ID, In - Out);
        }



    }
}
