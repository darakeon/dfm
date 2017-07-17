using System;
using System.Collections.Generic;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountData data = new AccountData();

        public IList<Move> GetMonthReport(Int32 id, Int32 dateMonth, Int32 dateYear)
        {
            return (IList<Move>)data.GetMonthReport(id, dateMonth, dateYear);
        }

        public Year GetYearReport(Int32 id, Int32 year)
        {
            return (Year)data.GetYearReport(id, year);
        }

        public void Close(Account account)
        {
            data.Close(account);
        }

        public Account SaveOrUpdate(Account account)
        {
            return (Account)data.SaveOrUpdate(account);
        }

        public Account SelectById(Int32 id)
        {
            return (Account)data.SelectById(id);
        }
    }
}
