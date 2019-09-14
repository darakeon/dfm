using System;
using System.Collections.Generic;
using System.Linq;
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

		public virtual Boolean IsOpen()
		{
			return EndDate == null;
		}
	}
}
