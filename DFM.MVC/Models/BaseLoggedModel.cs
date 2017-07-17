using System;
using System.Collections.Generic;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel
    {
        public BaseLoggedModel()
        {
            LateralAccountList = Current.User.AccountList;
        }

        public IList<Account> LateralAccountList { get; set; }
        public String CurrentMonth { get { return DateTime.Now.ToString("MMMM"); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }
    }
}