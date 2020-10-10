using System;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;

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
			checkUrl(account);
			checkLimits(account);
		}

		private void checkName(Account account)
		{
			if (String.IsNullOrEmpty(account.Name))
				throw Error.AccountNameRequired.Throw();

			if (account.Name.Length > MaxLen.AccountName)
				throw Error.TooLargeAccountName.Throw();

			var otherAccount = getByName(account.Name, account.User);

			var accountExistsForUser =
				otherAccount != null
					&& otherAccount.ID != account.ID;

			if (accountExistsForUser)
				throw Error.AccountNameAlreadyExists.Throw();
		}

		private void checkUrl(Account account)
		{
			if (String.IsNullOrEmpty(account.Url))
				throw Error.AccountUrlRequired.Throw();

			if (account.Url.Length > MaxLen.AccountUrl)
				throw Error.TooLargeAccountUrl.Throw();

			var regex = new Regex(@"^[a-z0-9_]*$");

			if (!regex.IsMatch(account.Url))
				throw Error.AccountUrlInvalid.Throw();

			var otherAccount = GetByUrl(account.Url, account.User);

			var accountUrlExistsForUser =
				otherAccount != null
					&& otherAccount.ID != account.ID;

			if (accountUrlExistsForUser)
				throw Error.AccountUrlAlreadyExists.Throw();
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
			if (!String.IsNullOrEmpty(account.Url))
				account.Url = account.Url.ToLower();

			if (account.ID == 0)
				account.BeginDate = account.User.Now();
		}

		private Account getByName(String name, User user)
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
			var accountList = Where(
				a => a.Url == url.ToLower()
				     && a.User.ID == user.ID
			);

			if (accountList.Count > 1)
				throw Error.DuplicatedAccountUrl.Throw();

			return accountList.SingleOrDefault();
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
	}
}
