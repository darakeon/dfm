using System;
using System.Collections.Generic;
using DFM.Core.Database;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            NHManager.NhInitialize(Current.User.AccountList);
            
            LateralAccountList = Current.User.AccountList;
        }

        public IList<Account> LateralAccountList { get; set; }
        public String CurrentMonth { get { return DateTime.Now.ToString("MMMM"); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }
    }
}