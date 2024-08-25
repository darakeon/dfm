using Keon.Util.DB;
using System;
using System.Collections.Generic;

namespace DFM.Entities
{
	public class Order : IEntityLong
	{
		public virtual Int64 ID { get; set; }
		public virtual Byte[] ExternalId { get; set; }

		public virtual DateTime Start { get; set; }
		public virtual DateTime End { get; set; }

		public virtual User User { get; set; }

		public virtual IList<Account> AccountList { get; set; } = new List<Account>();
		public virtual IList<Category> CategoryList { get; set; } = new List<Category>();

		public virtual Guid Guid
		{
			get => new(ExternalId);
			set => ExternalId = value.ToByteArray();
		}
	}
}
