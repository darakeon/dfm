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

            MonthList = MonthList.OrderBy(m => m.Number).ToList();
        }

        public IList<SimpleMonthJson> MonthList { get; private set; }

    }
}