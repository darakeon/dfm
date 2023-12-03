using System;
using System.Collections.Generic;
using System.Linq;
using DFM.API.Helpers.Models;

namespace DFM.API.Models
{
    internal class MovesExtractModel : BaseApiModel
    {
        public MovesExtractModel(string accountUrl, int id)
        {
            var monthDate = DateFromInt.GetDateMonth(id, now);
            var yearDate = DateFromInt.GetDateYear(id, now);

            var month = report.GetMonthReport(accountUrl, yearDate, monthDate);

            MoveList = month.MoveList.Reverse()
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
