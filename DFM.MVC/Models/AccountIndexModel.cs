using System.Collections.Generic;
using DFM.Core.Database;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Authentication;
using DFM.Core.Entities;
using NHibernate.Linq;

namespace DFM.MVC.Models
{
    public class AccountIndexModel : BaseModel
    {
        public AccountIndexModel()
        {
            Current.User.AccountList.ForEach(
                    a => NHManager.NhInitialize(a.MoveList)
                );

            AccountList = Current.User.AccountList;
        }

        public IList<Account> AccountList;
    }
}