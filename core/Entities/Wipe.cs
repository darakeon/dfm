using System;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Wipe : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual String HashedEmail { get; set; }
		public virtual String UsernameStart { get; set; }
		public virtual String DomainStart { get; set; }

		public virtual DateTime When { get; set; }
		public virtual RemovalReason Why { get; set; }

		public virtual String Password { get; set; }
		public virtual String S3 { get; set; }

		public override String ToString()
		{
			return $"[{ID}] {UsernameStart}...@{DomainStart}...";
		}
	}
}
