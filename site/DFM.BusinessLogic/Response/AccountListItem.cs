using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class AccountListItem
	{
		private AccountListItem(
			Account account,
			Decimal total,
			AccountSign sign,
			Boolean hasMoves
		)
		{
			Name = account.Name;
			Url = account.Url;
			Total = total;
			Start = account.BeginDate;
			End = account.EndDate;
			Sign = sign;
			HasMoves = hasMoves;
		}

		internal static AccountListItem Convert(
			Account account,
			Decimal total,
			AccountSign sign,
			Boolean hasMoves
		) => new AccountListItem(account, total, sign, hasMoves);

		public String Name { get; }
		public String Url { get; }
		public Decimal Total { get; }
		public DateTime Start { get; }
		public DateTime? End { get; }
		public AccountSign Sign { get; }
		public Boolean HasMoves { get; set; }
	}
}
