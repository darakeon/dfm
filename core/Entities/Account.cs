using System;
using System.Collections.Generic;
using DFM.Entities.Enums;
using Keon.Util.DB;
using DFM.Generic;

namespace DFM.Entities
{
	public class Account : IEntityLong
	{
		public Account()
		{
			SummaryList = new List<Summary>();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Name { get; set; }
		public virtual String Url { get; set; }

		public virtual Int32? RedLimitCents { get; set; }
		public virtual Int32? YellowLimitCents { get; set; }

		public virtual Currency? Currency { get; set; }

		public virtual Decimal? RedLimit
		{
			get => RedLimitCents.ToVisual();
			set => RedLimitCents = value.ToCents();
		}

		public virtual Decimal? YellowLimit
		{
			get => YellowLimitCents.ToVisual();
			set => YellowLimitCents = value.ToCents();
		}

		public virtual Boolean Open { get; set; }

		public virtual DateTime BeginDate { get; set; }
		public virtual DateTime? EndDate { get; set; }

		public virtual User User { get; set; }

		public virtual IList<Summary> SummaryList { get; set; }

		public virtual AccountSign GetSign(Decimal value)
		{
			var hasRed = RedLimit != null;
			var hasYellow = YellowLimit != null;

			if (hasRed && value < RedLimit)
				return AccountSign.Red;

			if (hasYellow && value < YellowLimit)
				return AccountSign.Yellow;

			if (hasRed || hasYellow)
				return AccountSign.Green;

			return AccountSign.None;
		}

		public override String ToString()
		{
			return $"[{ID}] {Name}";
		}
	}
}
