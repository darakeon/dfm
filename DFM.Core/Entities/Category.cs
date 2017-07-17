using System;

namespace DFM.Core.Entities
{
    public class Category : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }

        public virtual User User { get; set; }



        public override string ToString()
        {
            return Name;
        }

    }
}
