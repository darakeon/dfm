using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class MonthReport
	{
		public MonthReport(IList<Move> moveList, String accountUrl, Decimal accountTotal)
		{
			AccountTotal = accountTotal;
			MoveList = moveList
				.Select(m => MoveInfo.Convert4Report(m, accountUrl))
				.ToList();
		}

		public Decimal AccountTotal { get; set; }
		public IList<MoveInfo> MoveList { get; set; }
	}
}
