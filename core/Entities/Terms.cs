using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Terms : IEntityLong
	{
		public virtual Int64 ID { get; set; }
		public virtual String Language { get; set; }
		public virtual String Json { get; set; }

		public override String ToString()
		{
			return $"[{ID}]";
		}
	}
}
