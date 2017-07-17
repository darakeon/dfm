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
                    AjustValue();

                return FixValue;
            }
            set { FixValue = value; }
        }


        public virtual Boolean IsValid { get; set; }

        public virtual SummaryNature Nature { get; set; }


        public virtual Category Category { get; set; }

        public virtual Month Month { get; set; }

        public virtual Year Year { get; set; }


        internal protected virtual void AjustValue()
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

            SummaryData.SaveOrUpdate(this);
        }


        public override String ToString()
        {
            return String.Format("{0} - {1}",
                Category, (ISummarizable) Month ?? Year);
        }
    }
}
