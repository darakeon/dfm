using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public partial class Category : IEntityLong
	{
		public Category()
		{
			init();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Name { get; set; }
		public virtual Boolean Active { get; set; }

		public virtual User User { get; set; }
	}
}
