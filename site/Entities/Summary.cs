using System;
using Keon.Util.DB;
using DFM.Entities.Enums;

namespace DFM.Entities
{
	public partial class Summary : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual Int32 InCents { get; set; }
		public virtual Int32 OutCents { get; set; }

		public virtual Int32 Time { get; set; }
		public virtual SummaryNature Nature { get; set; }

		public virtual Boolean Broken { get; set; }

		public virtual Category Category { get; set; }
		public virtual Account Account { get; set; }
	}
}
