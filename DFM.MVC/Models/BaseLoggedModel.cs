using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Route;
using DFM.Core.Entities.Extensions;
using DFM.MVC.Authentication;
using DFM.Core.Entities;
using DFM.MVC.MultiLanguage;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel
    {
        public BaseLoggedModel()
        {
            LateralAccountList = Current.User.AccountList
                .Where(a => a.Open())
                .OrderByDescending(a => a.ID)
                .ToList();

            ActionName = RouteInfo.Current.RouteData
                .Values["action"].ToString();
        }

        public IList<Account> LateralAccountList { get; set; }

        public String CurrentMonth { get { return PlainText.GetMonthName(DateTime.Now.Month); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }

        public String ActionName { get; set; }

    
        
        public Menu MenuWith(String id, String text, String action, String controller)
        {
            return new Menu(id, text, action, controller);
        }

        public bool TooBigMeny(Int32 maxSize)
        {
            var accountNameList = LateralAccountList
                                    .Where(a => a.Open())
                                    .Select(a => a.Name)
                                    .ToList();
            
            var width = (accountNameList.Count - 1) * 20
                + accountNameList.Sum(
                    accountName => accountName.Length
                ) * 15;

            return width > maxSize;
        }
    }
}