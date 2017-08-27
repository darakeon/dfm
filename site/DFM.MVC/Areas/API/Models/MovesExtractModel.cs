using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Extensions;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.API.Models
{
    internal class MovesExtractModel : BaseApiModel
    {
        public MovesExtractModel(String accountUrl, Int32 id)
        {
            var monthDate = DateFromInt.GetDateMonth(id, Today);
            var yearDate = DateFromInt.GetDateYear(id, Today);

            MoveList =
                Report.GetMonthReport(accountUrl, monthDate, yearDate)
                    .Reverse()
                    .Select(m => new SimpleMoveJson(m, accountUrl))
                    .ToList();

            var account = Admin.GetAccountByUrl(accountUrl);

            Name = account.Name;
            Total = account.Sum();
        }

        public IList<SimpleMoveJson> MoveList { get; private set; }
        public String Name { get; private set; }
        public Double Total { get; private set; }

    }
}