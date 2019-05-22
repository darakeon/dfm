using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Acceptance : IEntity
	{
		public virtual Int32 ID { get; set; }

		public virtual DateTime CreateDate { get; set; }

		public virtual Boolean Accepted { get; set; }
		public virtual DateTime AcceptDate { get; set; }

		public virtual User User { get; set; }
		public virtual Contract Contract { get; set; }


	}
}