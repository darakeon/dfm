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
            }
            else if (!account.MoveList.Any())
            {
                var oldAccount = SelectById(account.ID);

                account.BeginDate = oldAccount.BeginDate;
                account.EndDate = oldAccount.EndDate;

                account.InList = oldAccount.InList;
                account.OutList = oldAccount.OutList;
            }
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


        public IList<Move> GetMonthReport(Int32 id, Int32 month, Int32 year)
        {
            var account = SelectById(id);

            return account.MoveList
                .Where(m => m.Date.Month == month
                        && m.Date.Year == year)
                .ToList();
        }


        public IDictionary<String, Double> GetYearReport(Int32 id, Int32 year)
        {
            var moveSumList = new Dictionary<String, Double>();

            var account = SelectById(id);

            makeYearList(moveSumList, account.InList, year, 1);
            makeYearList(moveSumList, account.OutList, year, -1);

            return moveSumList;
        }

        private static void makeYearList(IDictionary<String, Double> moveSumList, IEnumerable<Move> moveList, Int32 year, Int32 sign)
        {
            moveList
                .Where(m => m.Date.Year == year)
                .ForEach(m =>
                    moveSumList.SumItem(m.Month, sign * m.Value)
                );
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
