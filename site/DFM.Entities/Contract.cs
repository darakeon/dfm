using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Contract : IEntity
	{
		public virtual Int32 ID { get; set; }

		public virtual DateTime BeginDate { get; set; }
		public virtual String Version { get; set; }
	}
}
