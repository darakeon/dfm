using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Extensions.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    public class AccountService : BaseService<Account>
    {
        internal AccountService(DataAccess father, IRepository repository) : base(father, repository) { }

        public Account SaveOrUpdate(Account account)
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


        public IList<Move> GetMonthReport(Int32 id, Int16 dateMonth, Int16 dateYear)
        {
            var account = SelectById(id);


            var year = Father.Year.GetOrCreateYear(dateYear, account);

            if (year == null)
                return new List<Move>();


            var month = Father.Month.GetOrCreateMonth(dateMonth, year);

            return month == null
                ? new List<Move>()
                : month.MoveList();
        }


        public Year GetYearReport(Int32 accountid, Int16 dateYear)
        {
            var account = SelectById(accountid);

            var year = Father.Year.GetOrCreateYear(dateYear, account);

            return nonFuture(year);
        }

        private static Year nonFuture(Year year)
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


        public void Close(Account account)
        {
            if (account == null) return;

            if (!account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantCloseEmptyAccount);

            account.EndDate = DateTime.Now;
            SaveOrUpdate(account);
        }


        public new void Delete(Account account)
        {
            if (account == null) return;

            if (account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantDeleteAccountWithMoves);

            base.Delete(account);
        }

    }
}
