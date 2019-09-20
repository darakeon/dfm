using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class MonthReport
	{
		public MonthReport(Decimal accountTotal, IList<Move> moveList)
		{
			AccountTotal = accountTotal;
			MoveList = moveList
				.Select(MoveInfo.Convert4Report)
				.ToList();
		}

		public Decimal AccountTotal { get; set; }
		public IList<MoveInfo> MoveList { get; set; }
	}
}
