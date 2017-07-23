using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Route;
using DFM.Authentication;
using DFM.Entities.Extensions;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Views;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel
    {
        public BaseLoggedModel()
        {
            if (Current.IsAuthenticated)
            {
                LateralAccountList =
                    Current.User.AccountList
                        .Where(a => a.IsOpen())
                        .OrderBy(a => a.Name)
                        .ToList();
            }

            ActionName = RouteInfo.Current.RouteData == null
                ? String.Empty
                : RouteInfo.Current.RouteData.Values["action"].ToString();
        }

        public IList<Account> LateralAccountList { get; set; }

        public String CurrentMonth { get { return MultiLanguage.GetMonthName(DateTime.Now.Month); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }

        public String ActionName { get; set; }

    
        
        public Menu MenuWith(String id, String text, String action, String controller)
        {
            return new Menu(id, text, action, controller);
        }

        public bool TooBigMeny(Int32 maxSize)
        {
            var accountNameList = LateralAccountList
                                    .Where(a => a.IsOpen())
                                    .Select(a => a.Name)
                                    .ToList();

            var width = (accountNameList.Count - 1) * 20
                + accountNameList.Sum(
                    accountName => accountName.Length
                ) * 15;

            return width > maxSize;
        }


        protected readonly Current Current = Auth.Current; 

    }
}