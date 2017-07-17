using System;

namespace DFM.Core.Entities.AccountDetails
{
    public class Charge
    {
        public virtual Int32 ID { get; set; }

        public virtual Double DailySpend { get; set; }
        public virtual Double MaxValue { get; set; }

        public virtual Account Account { get; set; }

    }
}
