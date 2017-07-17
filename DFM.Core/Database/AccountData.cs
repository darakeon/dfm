using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    public class AccountData : BaseData<Account>
    {
        public Account SaveOrUpdate(Account account, User user)
        {
            Validate(account, user);

            account.BeginDate = DateTime.Now;
            account.User = user;

            return base.SaveOrUpdate(account);
        }

        public void Validate(Account account, User user)
        {
            if (SelectByName(account.Name, user) != null)
            {
                throw new CoreValidationException("Already Exists.");
            }
        }

        public override Account SaveOrUpdate(Account obj)
        {
            throw new CoreValidationException("Use the option with parameter User");
        }


        
        public Account SelectByName(string name, User user)
        {
            IList<Account> userList = Session
                .CreateCriteria(typeof(Account))
                .List<Account>()
                .Where(a => a.Name == name)
                .ToList();

            if (userList.Count > 1)
                throw new CoreValidationException("There is more than one account.");

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


        public IDictionary<String, Double> GetYearReport(int id, int? year)
        {
            var moveSumList = new Dictionary<String, Double>();


            var account = SelectById(id);

            var moveList = account.MoveList
                    .Where(m => m.Date.Year == year)
                    .Select(m => m)
                    .ToList();


            foreach (var move in moveList)
            {
                var sum = move.DetailList.Sum(d => d.Value);

                if (moveSumList.ContainsKey(move.Month))
                    moveSumList[move.Month] += sum;
                else
                    moveSumList.Add(move.Month, sum);
            }


            return moveSumList;
        }
    }
}
