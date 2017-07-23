using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.BusinessLogic.Exceptions;
using NHibernate;

namespace DFM.BusinessLogic.Services
{
    internal class AccountService : BaseService<Account>
    {
        internal AccountService(IRepository<Account> repository) : base(repository) { }

        internal Account SaveOrUpdate(Account account)
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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.AccountNameRequired);

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
            // TODO: use Current User
            var oldAccount = SelectByName(account.Name, account.User);

            if (oldAccount == null)
            {
                account.BeginDate = DateTime.Now;
                return;
            }

            if (!account.YearList.Any())
                account.YearList = oldAccount.YearList;

            if (account.BeginDate == DateTime.MinValue)
                account.BeginDate = oldAccount.BeginDate;

            if (account.IsOpen())
                account.EndDate = oldAccount.EndDate;

        }


        internal Account SelectByName(String name, User user)
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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedAccountName);
            }
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


        internal void Close(String name, User user)
        {
            var account = SelectByName(name, user);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            if (!account.IsOpen())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.ClosedAccount);

            if (!account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantCloseEmptyAccount);

            account.EndDate = DateTime.Now;

            SaveOrUpdate(account);
        }


        internal void Delete(String name, User user)
        {
            var account = SelectByName(name, user);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            if (account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantDeleteAccountWithMoves);

            Delete(account);
        }

    }
}
