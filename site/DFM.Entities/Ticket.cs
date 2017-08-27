using System;
using DFM.Entities.Bases;
using DFM.Generic;

namespace DFM.Entities
{
    public class Ticket : IEntity
    {
        public Ticket()
        {
            init();
        }

        private void init()
        {
            Active = true;
        }


        
        public virtual Int32 ID { get; set; }

        public virtual String Key { get; set; }
        public virtual TicketType Type { get; set; }
        
        public virtual DateTime Creation { get; set; }
        public virtual DateTime? Expiration { get; set; }
        public virtual Boolean Active { get; set; }

        public virtual User User { get; set; }

    }
}
