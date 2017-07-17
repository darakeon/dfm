using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class AccountIndexModel : BaseLoggedModel
    {
        public AccountIndexModel(Boolean open = true)
        {
            AccountList = Current.User.AccountList
                .Where(a => a.Open == open)
                .ToList();
        }

        public IList<Account> AccountList;
    }
}