using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Extensions;
using DFM.Entities;

namespace DFM.MVC.Models
{
    public class AccountIndexModel : BaseLoggedModel
    {
        public AccountIndexModel(Boolean open = true)
        {
            AccountList = 
                Current.User.AccountList
                    .Where(a => a.IsOpen() == open)
                    .OrderBy(a => a.Name)
                    .ToList();
        }

        public IList<Account> AccountList { get; set; }
    }
}