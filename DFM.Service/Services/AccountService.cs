using System;
using System.Collections.Generic;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    public class AccountService : IAccountService
    {
        public IList<Move> GetMonthReport(Int32 id, Int32 dateMonth, Int32 dateYear)
        {
            return (IList<Move>)AccountData.GetMonthReport(id, dateMonth, dateYear);
        }

        public Year GetYearReport(Int32 id, Int32 year)
        {
            return (Year)AccountData.GetYearReport(id, year);
        }

        public void Close(Account account)
        {
            AccountData.Close(account);
        }

        public void Delete(Account account)
        {
            AccountData.Delete(account);
        }

        public Account SaveOrUpdate(Account account)
        {
            return (Account)AccountData.SaveOrUpdate(account);
        }

        public Account SelectById(Int32 id)
        {
            return (Account)AccountData.SelectById(id);
        }
    }
}
