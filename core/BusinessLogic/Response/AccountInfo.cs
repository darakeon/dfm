using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class AccountInfo
	{
		public AccountInfo() { }

		private AccountInfo(Account account)
		{
			Name = account.Name;
			OriginalUrl = account.Url;

			YellowLimit = account.YellowLimit;
			RedLimit = account.RedLimit;
			Currency = account.Currency;

			IsOpen = account.Open;
			Start = account.BeginDate;
			End = account.EndDate;
		}

		internal static AccountInfo Convert(Account account)
			=> new(account);

		internal void Update(Account account)
		{
			account.Name = Name;
			account.YellowLimit = YellowLimit;
			account.RedLimit = RedLimit;
			account.Currency = Currency;
		}

		public String OriginalUrl { get; set; }

		public String Name { get; set; }
		public Decimal? YellowLimit { get; set; }
		public Decimal? RedLimit { get; set; }
		public Currency? Currency { get; set; }
		public Boolean IsOpen { get; set; }
		public DateTime Start { get; set; }
		public DateTime? End { get; set; }

		public Boolean HasLimit => RedLimit != null || YellowLimit != null;
	}
}
