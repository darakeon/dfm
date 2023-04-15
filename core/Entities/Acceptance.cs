using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Acceptance : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual DateTime CreateDate { get; set; }

		public virtual Boolean Accepted { get; set; }
		public virtual DateTime AcceptDate { get; set; }

		public virtual User User { get; set; }
		public virtual Contract Contract { get; set; }

		public override String ToString()
		{
			return $"[{ID}] ({Contract}) by ({User}): {Accepted}";
		}
	}
}
