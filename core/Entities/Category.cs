using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Category : IEntityLong
	{
		public Category()
		{
			Active = true;
		}

		public virtual Int64 ID { get; set; }

		public virtual String Name { get; set; }
		public virtual Boolean Active { get; set; }

		public virtual User User { get; set; }

		public override String ToString()
		{
			return $"[{ID}] {Name}";
		}
	}
}
