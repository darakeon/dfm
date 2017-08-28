using System;
using DK.Generic.DB;

namespace DFM.Entities
{
    public partial class Category : IEntity
    {
        public Category()
        {
            init();
        }

        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual Boolean Active { get; set; }

        public virtual User User { get; set; }


        



    }
}
