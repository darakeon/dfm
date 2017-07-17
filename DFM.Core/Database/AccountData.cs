using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Helpers;
using NHibernate.Linq;

namespace DFM.Core.Database
{
    public class AccountData : BaseData<Account>
    {
        public AccountData()
        {
            Validate += validate;
            Complete += complete;
        }

        private void validate(Account account)
        {
            var otherAccount = SelectByName(account.Name, account.User);

            var accountExistsForUser = otherAccount != null
                                            && otherAccount.ID != account.ID;

            if (accountExistsForUser)
            {
                throw new CoreValidationException("AlreadyExists");
            }
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


        
        public Account SelectByName(String name, User user)
        {
            IList<Account> userList = Session
                .CreateCriteria(typeof(Account))
                .List<Account>()
                .Where(a => a.Name == name)
                .ToList();

            if (userList.Count > 1)
                throw new CoreValidationException("DuplicatedName");

            return userList.SingleOrDefault();
        }


        public IList<Move> GetMonthReport(Int32 id, Int32 dateMonth, Int32 dateYear)
        {
            var account = SelectById(id);


            var year = account.YearList.SingleOrDefault(y => y.Time == dateYear);

            if (year == null)
                return new List<Move>();


            var month = year.MonthList.SingleOrDefault(y => y.Time == dateMonth);

            if (month == null)
                return new List<Move>();


            return month.MoveList;
        }


        public Year GetYearReport(Int32 id, Int32 year)
        {
            var account = SelectById(id);

            return account.YearList
                .SingleOrDefault(y => y.Time == year);
        }



        public void Close(Account account)
        {
            if (account != null)
            {
                account.EndDate = DateTime.Now;
                SaveOrUpdate(account);
            }
        }
    }
}
