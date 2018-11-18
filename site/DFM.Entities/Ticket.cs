using System;
using DFM.Entities.Enums;
using DK.Generic.DB;

namespace DFM.Entities
{
	public partial class Ticket : IEntity
	{
		public Ticket()
		{
			init();
		}


		public virtual Int32 ID { get; set; }

		public virtual String Key { get; set; }
		public virtual TicketType Type { get; set; }

		public virtual DateTime Creation { get; set; }
		public virtual DateTime? Expiration { get; set; }
		public virtual Boolean Active { get; set; }

		public virtual Boolean ValidTFA { get; set; }

		public virtual User User { get; set; }

	}
}
