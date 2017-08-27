using System;
using Ak.Generic.DB;
using DFM.Entities.Bases;
using DFM.Generic;

namespace DFM.Entities
{
    public class Detail : IEntity
    {
        public Detail()
        {
            init();
        }

        private void init()
        {
            Amount = 1;
        }



        public virtual Int32 ID { get; set; }
        
        public virtual String Description { get; set; }
        public virtual Int16 Amount { get; set; }
        public virtual Double Value { get; set; }

        public virtual Move Move { get; set; }
        public virtual Schedule Schedule { get; set; }


        public override String ToString()
        {
            return String.Format("[{0}] {1}", ID, Description);
        }

        public virtual Int64 FakeID
        {
            get
            {
                return ID * Constants.FakeID;
            }
            set
            {
                if (value % Constants.FakeID != 0)
                    throw new DFMException("Get back!");

                ID = (Int32)(value / Constants.FakeID);
            }
        }

    }
}
