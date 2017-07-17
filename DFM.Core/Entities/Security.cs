using System;
using DFM.Core.Enums;
using DFM.Core.Entities.Bases;

namespace DFM.Core.Entities
{
    public class Security : IEntity
    {
        public virtual Int32 ID { get; set; }
        
        public virtual String Guid { get; set; }
        public virtual Boolean Active { get; set; }
        public virtual DateTime Expire { get; set; }
        public virtual SecurityAction Action { get; set; }
        public virtual Boolean Sent { get; set; }

        public virtual String Layout { get; set; }
        public virtual String Subject { get; set; }

        public virtual User User { get; set; }

    }
}
