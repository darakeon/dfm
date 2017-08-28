using System;
using DK.Generic.DB;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public partial class Summary : IEntity
    {
        public Summary()
        {
            init();
        }

        public Summary(Month month, Category category) : this()
        {
            init(month, category);
        }

        public Summary(Year year, Category category)
        {
            init(year, category);
        }



        public virtual Int32 ID { get; set; }

        public virtual Int32 InCents { get; set; }
        public virtual Int32 OutCents { get; set; }
        public virtual SummaryNature Nature { get; set; }

        public virtual Boolean Broken { get; set; }

        public virtual Category Category { get; set; }
        public virtual Month Month { get; set; }
        public virtual Year Year { get; set; }


		
    }
}
