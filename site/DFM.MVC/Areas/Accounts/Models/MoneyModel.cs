using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoneyModel : BaseModel
    {
        public void DeleteMove(Int32 id)
        {
            var move = getMove(id);

            if (move == null)
            {
                ErrorAlert.Add("MoveNotFound");
            }
            else
            {
                var result = Money.DeleteMove(id);

                if (result.Error.IsWrong())
                {
                    var deleted = MultiLanguage.Dictionary["MoveDeletedWithoutEmail"];
                    var error = MultiLanguage.Dictionary[result.Error];
                    var message = String.Format(deleted, move.Description, error);
                    ErrorAlert.AddTranslated(message);
                }
                else
                {
                    var deleted = MultiLanguage.Dictionary["MoveDeleted"];
                    var message = String.Format(deleted, move.Description);
                    ErrorAlert.AddTranslated(message);
                }

                ReportUrl = (move.Out ?? move.In).Url();
            }
        }

        private static Move getMove(int id)
        {
            try
            {
                return Money.GetMoveById(id);
            }
            catch (DFMCoreException)
            {
                return null;
            }
        }

        public Int32? ReportUrl;

    }

}