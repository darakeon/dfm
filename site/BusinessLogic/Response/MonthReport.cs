using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class MonthReport
	{
		public MonthReport(
			String accountUrl,
			Decimal accountTotal, Decimal foreseenTotal,
			IList<Move> moveList, IList<Move> foreseenList
		)
		{
			AccountTotal = accountTotal;
			MoveList = moveList
				.Select(m => MoveInfo.Convert4Report(m, accountUrl, false))
				.ToList();

			ForeseenTotal = foreseenTotal;
			ForeseenList = foreseenList
				.Select(m => MoveInfo.Convert4Report(m, accountUrl, true))
				.ToList();
		}

		public Decimal AccountTotal { get; }
		public Decimal ForeseenTotal { get; }

		public IList<MoveInfo> MoveList { get; }
		public IList<MoveInfo> ForeseenList { get; }
	}
}
