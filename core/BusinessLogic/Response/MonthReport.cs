using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class MonthReport
	{
		public MonthReport(
			String accountUrl,
			Decimal accountTotal, Decimal foreseenTotal,
			AccountSign accountSign, AccountSign foreseenSign,
			IList<Move> moveList, IList<Move> foreseenList,
			Boolean accountHasMoves
		)
		{
			AccountTotal = accountTotal;
			AccountTotalSign = accountSign;
			MoveList = moveList
				.Select(m => MoveInfo.Convert4Report(m, accountUrl, false))
				.ToList();

			ForeseenTotal = foreseenTotal;
			ForeseenTotalSign = foreseenSign;
			ForeseenList = foreseenList
				.Select(m => MoveInfo.Convert4Report(m, accountUrl, true))
				.ToList();

			AccountHasMoves = accountHasMoves;
		}

		public Decimal AccountTotal { get; }
		public Decimal ForeseenTotal { get; }

		public AccountSign AccountTotalSign { get; }
		public AccountSign ForeseenTotalSign { get; }

		public IList<MoveInfo> MoveList { get; }
		public IList<MoveInfo> ForeseenList { get; }

		public Boolean AccountHasMoves { get; }
	}
}
