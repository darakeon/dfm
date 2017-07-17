using System;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;
using DFM.Core.Database;
using DFM.Core.Helpers;

namespace DFM.Core.Entities
{
    public class Summary : IEntity
    {
        public virtual Int32 ID { get; set; }


        public virtual Double FixValue { get; set; }

        public virtual Double Value
        {
            get
            {
                if (!IsValid)
                    SummaryData.AjustValue(this);

                return FixValue;
            }
            set { FixValue = value; }
        }


        public virtual Boolean IsValid { get; set; }

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
