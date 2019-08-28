using System;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using Keon.NHibernate.Base;
using NHibernate;

namespace DFM.BusinessLogic.Repositories
{
	internal class AccountRepository : BaseRepository<Account>
	{
		internal Account SaveOrUpdate(Account account)
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
				throw DFMCoreException.WithMessage(DfMError.AccountNameRequired);

			if (account.Name.Length > MaxLen.Account_Name)
				throw DFMCoreException.WithMessage(DfMError.TooLargeAccountName);

			var otherAccount = getByName(account.Name, account.User);

			var accountExistsForUser =
				otherAccount != null
					&& otherAccount.ID != account.ID;

			if (accountExistsForUser)
				throw DFMCoreException.WithMessage(DfMError.AccountNameAlreadyExists);
		}

		private void checkUrl(Account account)
		{
			if (String.IsNullOrEmpty(account.Url))
				throw DFMCoreException.WithMessage(DfMError.AccountUrlRequired);

			if (account.Url.Length > MaxLen.Account_Url)
				throw DFMCoreException.WithMessage(DfMError.TooLargeAccountUrl);

			var regex = new Regex(@"^[a-z0-9_]*$");

			if (!regex.IsMatch(account.Url))
				throw DFMCoreException.WithMessage(DfMError.AccountUrlInvalid);


			var otherAccount = GetByUrl(account.Url, account.User);

			var accountUrlExistsForUser =
				otherAccount != null
					&& otherAccount.ID != account.ID;

			if (accountUrlExistsForUser)
				throw DFMCoreException.WithMessage(DfMError.AccountUrlAlreadyExists);
		}

		private static void checkLimits(Account account)
		{
			if (account.RedLimit == null || account.YellowLimit == null)
				return;

			if (account.RedLimit > account.YellowLimit)
				throw DFMCoreException.WithMessage(DfMError.RedLimitAboveYellowLimit);
		}



		private void complete(Account account)
		{
			if (!String.IsNullOrEmpty(account.Url))
				account.Url = account.Url.ToLower();

			var oldAccount = Get(account.ID);

			if (oldAccount == null)
			{
				account.BeginDate = account.User.Now();
				return;
			}

			if (!account.YearList.Any())
				account.YearList = oldAccount.YearList;

			if (account.BeginDate == DateTime.MinValue)
				account.BeginDate = oldAccount.BeginDate;

			if (account.IsOpen())
				account.EndDate = oldAccount.EndDate;

		}


		private Account getByName(String name, User user)
		{
			try
			{
				return SingleOrDefault(
						a => a.Name == name
							&& a.User.ID == user.ID
					);
			}
			catch (NonUniqueResultException)
			{
				throw DFMCoreException.WithMessage(DfMError.DuplicatedAccountName);
			}
		}

		internal Account GetByUrl(String url, User user)
		{
			try
			{
				return SingleOrDefault(
						a => a.Url == url.ToLower()
							&& a.User.ID == user.ID
					);
			}
			catch (NonUniqueResultException)
			{
				throw DFMCoreException.WithMessage(DfMError.DuplicatedAccountUrl);
			}
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
				throw DFMCoreException.WithMessage(DfMError.InvalidAccount);

			if (!account.IsOpen())
				throw DFMCoreException.WithMessage(DfMError.ClosedAccount);

			if (!account.HasMoves())
				throw DFMCoreException.WithMessage(DfMError.CantCloseEmptyAccount);

			account.EndDate = account.User.Now();

			SaveOrUpdate(account);
		}


		internal void Delete(String url, User user)
		{
			var account = GetByUrl(url, user);

			if (account == null)
				throw DFMCoreException.WithMessage(DfMError.InvalidAccount);

			if (account.HasMoves())
				throw DFMCoreException.WithMessage(DfMError.CantDeleteAccountWithMoves);

			Delete(account);
		}

	}
}
