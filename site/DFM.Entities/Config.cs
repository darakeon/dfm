using System;
using Ak.Generic.DB;

namespace DFM.Entities
{
    public class Config : IEntity
    {
        public virtual Int32 ID { get; set; }

        public virtual String Language { get; set; }
        public virtual String TimeZone { get; set; }
        public virtual Boolean SendMoveEmail { get; set; }
        public virtual Boolean UseCategories { get; set; }


        public override String ToString()
        {
            return String.Format("[{0}]", ID);
        }

    }
}
