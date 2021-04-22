using System;
using System.Collections.Generic;
using DFM.Entities.Enums;

namespace DFM.Entities
{
	public partial class Account
	{
		private void init()
		{
			SummaryList = new List<Summary>();
		}

		public override String ToString()
		{
			return $"[{ID}] {Name}";
		}

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
	}
}
