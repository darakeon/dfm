using System;
using Keon.Util.DB;
using DFM.Entities.Enums;

namespace DFM.Entities
{
	public partial class Security : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual String Token { get; set; }
		public virtual Boolean Active { get; set; }
		public virtual DateTime Expire { get; set; }
		public virtual SecurityAction Action { get; set; }
		public virtual Boolean Sent { get; set; }

		// Just for helping business logic
		public virtual String Path { get; set; }

		public virtual User User { get; set; }
	}
}
