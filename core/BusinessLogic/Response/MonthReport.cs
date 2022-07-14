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
			Account account,
			Decimal accountTotal, Decimal foreseenTotal,
			IList<Move> moveList, IList<Move> foreseenList,
			Boolean accountHasMoves
		)
		{
			AccountTotal = accountTotal;
			if (account.User.Settings.UseAccountsSigns)
				AccountTotalSign = account.GetSign(accountTotal);
			MoveList = moveList
				.Select(m => MoveInfo.Convert4Report(m, account.Url, false))
				.ToList();

			ForeseenTotal = foreseenTotal;
			if (account.User.Settings.UseAccountsSigns)
				ForeseenTotalSign = account.GetSign(foreseenTotal);
			ForeseenList = foreseenList
				.Select(m => MoveInfo.Convert4Report(m, account.Url, true))
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
