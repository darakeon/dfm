using System;
using Ak.MVC.Route;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
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
        public Entities.Account Account { get; private set; }

    }

}