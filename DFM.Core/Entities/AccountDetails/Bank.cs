using System;
using DFM.Core.Entities.Bases;
using DFM.Core.Entities.Extensions;
using DFM.Core.Enums;

namespace DFM.Core.Entities.AccountDetails
{
    public class Bank : IAccountDetail
    {
        public virtual Int32 ID { get; set; }

        public virtual Double Value { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime Next { get; set; }

        public virtual ScheduleFrequency Frequency { get; set; }

        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }

    }
}
