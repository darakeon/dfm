using System;
using Keon.Util.DB;
using Newtonsoft.Json;

namespace DFM.Entities
{
	public class Terms : IEntityLong
	{
		public virtual Int64 ID { get; set; }
		public virtual String Language { get; set; }
		public virtual String Json { get; set; }
	}
}
