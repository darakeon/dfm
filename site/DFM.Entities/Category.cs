using System;
using System.Collections.Generic;
using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class Category : IEntity
    {
        public Category()
        {
            init();
        }

        private void init()
        {
            Active = true;
        }



        public virtual Int32 ID { get; set; }

        public virtual String Name { get; set; }
        public virtual Boolean Active { get; set; }

        public virtual User User { get; set; }


        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Name);
        }


    }
}
