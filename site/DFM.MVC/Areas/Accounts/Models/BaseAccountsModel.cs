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

            CurrentAccountUrl = routeInfo.RouteData.Values["accounturl"].ToString();

            Account = Admin.GetAccountByUrl(CurrentAccountUrl);
        }

        public String CurrentAccountUrl { get; private set; }
        public Account Account { get; private set; }

    }

}