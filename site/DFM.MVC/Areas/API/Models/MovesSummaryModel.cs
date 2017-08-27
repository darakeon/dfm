using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.API.Models
{
    internal class MovesSummaryModel : BaseApiModel
    {
        public MovesSummaryModel(String accountUrl, Int16 id)
        {
            var yearDate = DateFromInt.GetDateYear(id, Today);

            MonthList =
                Report.GetYearReport(accountUrl, yearDate)
                    .MonthList
                    .Select(m => new SimpleMonthJson(m))
                    .ToList();

            for (Int16 ym = 1; ym < 13; ym++)
            {
                if (MonthList.All(m => m.Number != ym))
                {
                    MonthList.Add(new SimpleMonthJson(ym));
                }
            }

            MonthList = MonthList
                .OrderByDescending(m => m.Number)
                .ToList();

            var account = Admin.GetAccountByUrl(accountUrl);

            Name = account.Name;
            Total = account.Total();
        }

        public IList<SimpleMonthJson> MonthList { get; private set; }
        public String Name { get; private set; }
        public Double Total { get; private set; }

    }
}