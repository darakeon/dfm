using System;
using System.Collections.Generic;
using System.Linq;
using DFM.API.Helpers.Models;

namespace DFM.API.Models
{
    internal class AccountsExtractModel : BaseApiModel
    {
        public AccountsExtractModel(String accountUrl, Int16? year, Int16? month)
        {
            var monthDate = DateFromInt.GetDateMonth(month, now);
            var yearDate = DateFromInt.GetDateYear(year, now);

            var extract = report.GetMonthReport(accountUrl, yearDate, monthDate);

            MoveList = extract.MoveList.Reverse()
                .Select(m => new SimpleMoveJson(m, accountUrl))
                .ToList();

            var account =
                admin.GetAccountList(true)
                    .SingleOrDefault(a => a.Url == accountUrl)
                ?? admin.GetAccountList(false)
                    .Single(a => a.Url == accountUrl);

            Title = account.Name;
            Total = account.Total;
            CanCheck = moveCheckingEnabled;
        }

        public IList<SimpleMoveJson> MoveList { get; }
        public string Title { get; }
        public decimal Total { get; }
        public bool CanCheck { get; }
    }
}
