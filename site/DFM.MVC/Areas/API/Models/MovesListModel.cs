using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Areas.API.Jsons;

namespace DFM.MVC.Areas.API.Models
{
    internal class MovesListModel : BaseApiModel
    {
        public MovesListModel(String accountUrl, Int16 monthDate, Int16 yearDate)
        {
            MoveList =
                Report.GetMonthReport(accountUrl, monthDate, yearDate)
                    .Select(m => new MoveJson(m))
                    .ToList();

            var account = Admin.GetAccountByUrl(accountUrl);

            MoveList.Where(m => m.AccountOut == account.Name)
                .ToList()
                .ForEach(m => m.Total = -m.Total);
        }

        public IList<MoveJson> MoveList { get; private set; }
    }
}