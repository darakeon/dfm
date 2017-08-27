using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.API.Models
{
    internal class MovesListModel : BaseApiModel
    {
        public MovesListModel(String accountUrl, Int32? id)
        {
            var monthDate = DateFromInt.GetDateMonth(id, Today);
            var yearDate = DateFromInt.GetDateYear(id, Today);

            MoveList =
                Report.GetMonthReport(accountUrl, monthDate, yearDate)
                    .Select(m => new MoveJson(m))
                    .ToList();

            MoveList.Where(m => m.AccountOut == accountUrl)
                .ToList()
                .ForEach(m => m.Total = -m.Total);
        }

        public IList<MoveJson> MoveList { get; private set; }

    }
}