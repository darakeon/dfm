using System;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Wipe : IEntityLong
	{
		public virtual Int64 ID { get; set; }
		public virtual String Email { get; set; }
		public virtual DateTime When { get; set; }
		public virtual RemovalReason Why { get; set; }
		public virtual String S3 { get; set; }
		public virtual String Password { get; set; }
	}
}
