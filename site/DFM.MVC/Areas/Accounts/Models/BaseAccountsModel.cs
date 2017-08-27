using System;
using Ak.MVC.Route;
using DFM.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class BaseAccountsModel : BaseLoggedModel
    {
        public BaseAccountsModel()
        {
            var routeInfo = new RouteInfo();

            AccountUrl = routeInfo.RouteData.Values["accounturl"].ToString();

            Account = Admin.GetAccountByUrl(AccountUrl);
        }

        public String AccountUrl { get; private set; }
        public Account Account { get; private set; }

    }

}