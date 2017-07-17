using System;
using System.Collections.Generic;
using Ak.MVC.Route;
using DFM.MVC.Authentication;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel
    {
        public BaseLoggedModel()
        {
            LateralAccountList = Current.User.AccountList;
            ActionName = RouteInfo.Current.RouteData.Values["action"].ToString();
        }

        public IList<Account> LateralAccountList { get; set; }
        public String CurrentMonth { get { return DateTime.Now.ToString("MMMM"); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }

        public String ActionName { get; set; }
    }
}