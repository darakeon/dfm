using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class AccountListItem
	{
		private AccountListItem(Account account)
		{
			Name = account.Name;
			Url = account.Url;
			Total = account.Total();
			Start = account.BeginDate;
			End = account.EndDate;
			Sign = account.Sign();
			HasMoves = account.HasMoves();
		}

		internal static AccountListItem Convert(Account account)
			=> new AccountListItem(account);

		public String Name { get; }
		public String Url { get; }
		public Decimal Total { get; }
		public DateTime Start { get; }
		public DateTime? End { get; }
		public AccountSign Sign { get; }
		public Boolean HasMoves { get; set; }
	}
}
