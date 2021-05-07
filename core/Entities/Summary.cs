using System;
using Keon.Util.DB;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
	public class Summary : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual Int32 InCents { get; set; }
		public virtual Int32 OutCents { get; set; }

		public virtual Int32 Time { get; set; }
		public virtual SummaryNature Nature { get; set; }

		public virtual Boolean Broken { get; set; }

		public virtual Category Category { get; set; }
		public virtual Account Account { get; set; }

		public virtual Decimal In
		{
			get => InCents.ToVisual();
			set => InCents = value.ToCents();
		}

		public virtual Decimal Out
		{
			get => OutCents.ToVisual();
			set => OutCents = value.ToCents();
		}

		public virtual Decimal Value()
		{
			return Math.Round(In - Out, 2);
		}

		public virtual User User()
		{
			return Account.User;
		}

		public override String ToString()
		{
			return $"[{ID}] {In - Out}";
		}
	}
}
