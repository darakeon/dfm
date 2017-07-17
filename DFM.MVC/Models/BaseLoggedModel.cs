using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Route;
using DFM.MVC.Authentication;
using DFM.Core.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel
    {
        public BaseLoggedModel()
        {
            LateralAccountList = Current.User.AccountList
                .Where(a => a.Open)
                .ToList();

            ActionName = RouteInfo.Current.RouteData
                .Values["action"].ToString();
        }

        public IList<Account> LateralAccountList { get; set; }
        public String CurrentMonth { get { return PlainText.GetMonthName(DateTime.Now.Month); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }

        public String ActionName { get; set; }
    }
}