using System;
using System.Linq;
using DFM.Entities;
using DFM.Extensions;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    internal class AccountService : BaseService<Account>
    {
        internal AccountService(
            IRepository repository) : base(repository) { }

        internal Account SaveOrUpdate(Account account)
        {
            return SaveOrUpdate(account, complete, validate);
        }


        private static void validate(Account account)
        {
            checkName(account);
            checkLimits(account);
        }

        private static void checkName(Account account)
        {
            var otherAccount = SelectByName(account.Name, account.User);

            var accountExistsForUser = otherAccount != null
                                       && otherAccount.ID != account.ID;

            if (accountExistsForUser)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.AccountAlreadyExists);
        }

        private static void checkLimits(Account account)
        {
            if (account.RedLimit == null || account.YellowLimit == null)
                return;

            if (account.RedLimit > account.YellowLimit)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.RedLimitAboveYellowLimit);
        }



        private void complete(Account account)
        {
            if (account.ID == 0)
            {
                account.BeginDate = DateTime.Now;
                return;
            }

            var oldAccount = SelectById(account.ID);

            if (!account.YearList.Any())
                account.YearList = oldAccount.YearList;

            if (account.BeginDate == DateTime.MinValue)
                account.BeginDate = oldAccount.BeginDate;

            if (account.EndDate == null)
                account.EndDate = oldAccount.EndDate;

        }



        internal static Account SelectByName(String name, User user)
        {
            var accountList = user.AccountList
                .Where(a => a.Name == name)
                .ToList();

            if (accountList.Count > 1)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedAccountName);

            return accountList.SingleOrDefault();
        }





        internal Year NonFuture(Year year)
        {         
            if (year.Time >= DateTime.Today.Year)
            {
                //prevent from saving and destroying the original element
                var currentYear = year.Clone();

                currentYear.MonthList =
                    currentYear.MonthList
                        .Where(m => m.Time <= DateTime.Today.Month)
                        .ToList();

                return currentYear;
            }

            return year;
        }


        internal void Close(Account account)
        {
            if (account == null) return;

            if (!account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantCloseEmptyAccount);

            account.EndDate = DateTime.Now;
            SaveOrUpdate(account);
        }


        internal new void Delete(Account account)
        {
            if (account == null) return;

            if (account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantDeleteAccountWithMoves);

            base.Delete(account);
        }

    }
}
