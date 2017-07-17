using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;
using DFM.Core.Exceptions;

namespace DFM.Core.Database
{
    public class AccountData : BaseData<Account>
    {
		private AccountData() { }

        public static Account SaveOrUpdate(Account account)
        {
            return SaveOrUpdate(account, validate, complete);
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
            {
                throw DFMCoreException.WithMessage(ExceptionPossibilities.AccountAlreadyExists);
            }
        }

        private static void checkLimits(Account account)
        {
            if (account.RedLimit == null || account.YellowLimit == null)
                return;

            if (account.RedLimit >= account.YellowLimit)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.YellowLimitUnderRedLimit);
        }



        private static void complete(Account account)
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
            IList<Account> accountList = Session
                .CreateCriteria(typeof(Account))
                .List<Account>()
                .Where(a => a.Name == name)
                .ToList();

            if (accountList.Count > 1)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DuplicatedAccountName);

            return accountList.SingleOrDefault();
        }


        public static IList<Move> GetMonthReport(Int32 id, Int32 dateMonth, Int32 dateYear)
        {
            var account = SelectById(id);


            var year = account.GetYear(dateYear);

            if (year == null)
                return new List<Move>();


            var month = year.GetMonth(dateMonth);

            return month == null
                ? new List<Move>()
                : month.MoveList();
        }


        public static Year GetYearReport(Int32 accountid, Int16 dateYear)
        {
            var account = SelectById(accountid);

            var year = account.GetYear(dateYear);

            return year == null
                ? new Year { Account = account, Time = dateYear }
                : nonFuture(year);
        }

        private static Year nonFuture(Year year)
        {         
            if (year.Time == DateTime.Today.Year)
            {
                //prevent from saving and destroying the true element
                var currentYear = year.Clone();

                currentYear.MonthList =
                    currentYear.MonthList
                        .Where(m => m.Time <= DateTime.Today.Month)
                        .ToList();
            }

            return year;
        }


        public static void Close(Account account)
        {
            if (account == null) return;

            if (!account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantCloseEmptyAccount);

            account.EndDate = DateTime.Now;
            SaveOrUpdate(account);
        }


        public new static void Delete(Account account)
        {
            if (account == null) return;

            if (account.HasMoves())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CantDeleteAccountWithMoves);

            BaseData<Account>.Delete(account);
        }

    }
}
