using System;
using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class Ticket : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual String Key { get; set; }
        public virtual DateTime Creation { get; set; }
        public virtual DateTime Expiration { get; set; }
        public virtual Boolean Active { get; set; }

        public virtual User User { get; set; }

    }
}
