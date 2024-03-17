using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models
{
    internal class AccountsIndexModel : BaseApiModel
    {
        public AccountsIndexModel()
        {
            AccountList =
                admin.GetAccountList(true)
                    .OrderBy(a => a.Name)
                    .ToList();
        }

        public IList<AccountListItem> AccountList { get; }
    }
}
