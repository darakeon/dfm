using System;
using DFM.Core.Database;
using DFM.Core.Entities.Bases;
using DFM.Core.Enums;
using DFM.Core.Helpers;

namespace DFM.Core.Entities
{
    public class Summary : IEntity
    {
        public virtual Int32 ID { get; set; }



        public virtual Double Value { get; set; }

        public virtual Double SafeValue
        {
            get
            {
                if (!IsValid)
                    AjustValue();

                return Value;
            }
            protected internal set { Value = value; }
        }


        public virtual Boolean IsValid { get; set; }

        public virtual SummaryNature Nature { get; set; }



        public virtual Category Category { get; set; }
        
        public virtual Month Month { get; set; }
        
        public virtual Year Year { get; set; }



        public virtual void AjustValue()
        {
            switch (Nature)
            {
                case SummaryNature.Month:
                    Value = Month.CheckUp(Category); break;
                case SummaryNature.Year:
                    Value = Year.CheckUp(Category); break;
                default:
                    throw new DFMCoreException("SummaryNatureNotFound");
            }

            IsValid = true;

            new SummaryData().SaveOrUpdate(this);
        }


        
        public override String ToString()
        {
            return String.Format("{0} - {1}",
                Category, (ISummarizable) Month ?? Year);
        }
    }
}
