using System.Collections.Generic;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class AccountIndexModel : BaseLoggedModel
    {
        public AccountIndexModel()
        {
            AccountList = Current.User.AccountList;
        }

        public IList<Account> AccountList;
    }
}