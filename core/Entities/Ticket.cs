using System;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Ticket : IEntityLong
	{
		public Ticket()
		{
			Active = true;
		}

		public virtual Int64 ID { get; set; }

		public virtual String Key { get; set; }
		public virtual TicketType Type { get; set; }

		public virtual DateTime Creation { get; set; }
		public virtual DateTime LastAccess { get; set; }
		public virtual DateTime? Expiration { get; set; }
		public virtual Boolean Active { get; set; }

		public virtual Boolean ValidTFA { get; set; }

		public virtual User User { get; set; }

		public override String ToString()
		{
			return $"[{ID}] ({User}) {Active}";
		}
	}
}
