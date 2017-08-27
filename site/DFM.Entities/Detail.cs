using System;
using Ak.Generic.DB;

namespace DFM.Entities
{
    public partial class Detail : IEntity
    {
        public Detail()
        {
            init();
        }

        public virtual Int32 ID { get; set; }
        
        public virtual String Description { get; set; }
        public virtual Int16 Amount { get; set; }
		public virtual Int32 ValueCents { get; set; }

        public virtual Move Move { get; set; }
        public virtual Schedule Schedule { get; set; }

    }
}
