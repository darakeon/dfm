using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Extensions;
using DFM.MVC.Areas.Android.Jsons;

namespace DFM.MVC.Areas.Android.Models
{
    internal class AccountListModel : BaseJsonModel
    {
        public AccountListModel()
        {
            AccountList = 
                Current.User.AccountList
                    .Where(a => a.IsOpen())
                    .OrderBy(a => a.Name)
                    .Select(a => new AccountJson(a))
                    .ToList();
        }

        public IList<AccountJson> AccountList { get; private set; }

    }
}