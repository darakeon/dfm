using System;
using DFM.Core.Entities.Base;

namespace DFM.Core.Entities
{
    public class Category : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual Boolean Active { get; set; }

        public virtual User User { get; set; }


        public override String ToString()
        {
            return Name;
        }


    }
}
