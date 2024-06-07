using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;
using DFM.Generic;

namespace DFM.BusinessLogic.Repositories
{
	internal class AccountRepository : Repo<Account>
	{
		internal Account Save(Account account)
		{
			return SaveOrUpdate(account, complete, validate);
		}

		private void validate(Account account)
		{
			checkName(account);
			checkLimits(account);
		}

		private void checkName(Account account)
		{
			if (String.IsNullOrEmpty(account.Name))
				throw Error.AccountNameRequired.Throw();

			if (account.Name.Length > MaxLen.AccountName)
				throw Error.TooLargeAccountName.Throw();

			var otherAccount = GetByName(account.Name, account.User);

			var accountExistsForUser =
				otherAccount != null
					&& otherAccount.ID != account.ID;

			if (accountExistsForUser)
				throw Error.AccountNameAlreadyExists.Throw();
		}

		private static void checkLimits(Account account)
		{
			if (account.RedLimit == null || account.YellowLimit == null)
				return;

			if (account.RedLimit > account.YellowLimit)
				throw Error.RedLimitAboveYellowLimit.Throw();
		}

		private void complete(Account account)
		{
			if (account.Name != null)
				account.Url = generateUniqueUrl(account);

			if (account.ID == 0)
				account.BeginDate = account.User.Now();
		}

		private String generateUniqueUrl(Account account)
		{
			var url = account.Name.IntoUrl();
			for (var a = 2; getOtherAccount(url, account) != null; a++)
			{
				url = $"{account.Name.IntoUrl()}_{a}";
			}
			return url;
		}

		private Account getOtherAccount(String url, Account account)
		{
			var otherAccount = GetByUrl(url, account.User);

			return otherAccount?.ID == account.ID
				? null
				: otherAccount;
		}

		internal Account GetByName(String name, User user)
		{
			var accountList = Where(
					a => a.Name == name
						&& a.User.ID == user.ID
				);

			if (accountList.Count > 1)
				throw Error.DuplicatedAccountName.Throw();

			return accountList.SingleOrDefault();
		}

		internal Account GetByUrl(String url, User user)
		{
			return SingleOrDefault(
				a => a.Url == url
					&& a.User.ID == user.ID
			);
		}

		internal void Close(Account account)
		{
			if (!account.Open)
				throw Error.ClosedAccount.Throw();

			account.Open = false;
			account.EndDate = account.User.Now();

			Save(account);
		}

		internal void Reopen(Account account)
		{
			if (account.Open)
				throw Error.OpenedAccount.Throw();

			account.Open = true;
			account.EndDate = null;

			Save(account);
		}

		public IList<Account> Get(User user, Boolean open)
		{
			return NewQuery()
				.Where(a => a.User.ID == user.ID)
				.Where(a => a.Open == open)
				.OrderBy(a => a.Name)
				.List;
		}
	}
}
