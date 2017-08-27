using System;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoneyModel : BaseModel
    {
        public void DeleteMove(Int32 id)
        {
            var move = Money.GetMoveById(id);

            Money.DeleteMove(id);

            // TODO: implement messages on page head
            //var message = move == null
            //    ? MultiLanguage.Dictionary["MoveNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["MoveDeleted"], move.Description);

            ReportUrl = (move.In ?? move.Out).Url();
        }

        public Int32 ReportUrl;

    }

}