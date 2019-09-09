using System;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class AccountRepository : BaseRepositoryLong<Account>
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

			if (account.Name.Length > MaxLen.Account_Name)
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

			if (account.Url.Length > MaxLen.Account_Url)
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
			var accountList = SimpleFilter(
					a => a.Name == name
						&& a.User.ID == user.ID
				);

			if (accountList.Count > 1)
				throw Error.DuplicatedAccountName.Throw();

			return accountList.SingleOrDefault();
		}

		internal Account GetByUrl(String url, User user)
		{
			var accountList = SimpleFilter(
				a => a.Url == url.ToLower()
				     && a.User.ID == user.ID
			);

			if (accountList.Count > 1)
				throw Error.DuplicatedAccountUrl.Throw();

			return accountList.SingleOrDefault();
		}





		internal Year NonFuture(Year year)
		{
			var now = year.User().Now();

			if (year.Time >= now.Year)
			{
				//prevent from saving and destroying the original element
				var currentYear = year.Clone();

				currentYear.MonthList =
					currentYear.MonthList
						.Where(m => m.Time <= now.Month)
						.ToList();

				return currentYear;
			}

			return year;
		}


		internal void Close(Account account)
		{
			if (account == null)
				throw Error.InvalidAccount.Throw();

			if (!account.IsOpen())
				throw Error.ClosedAccount.Throw();

			if (!account.HasMoves())
				throw Error.CantCloseEmptyAccount.Throw();

			account.EndDate = account.User.Now();

			Save(account);
		}


		internal void Delete(String url, User user)
		{
			var account = GetByUrl(url, user);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			if (account.HasMoves())
				throw Error.CantDeleteAccountWithMoves.Throw();

			Delete(account);
		}

	}
}
