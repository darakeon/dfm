using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Route;
using DFM.Authentication;
using DFM.Entities.Extensions;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Views;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel : BaseModel
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




    }
}