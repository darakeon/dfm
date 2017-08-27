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

            var url = routeInfo.RouteData.Values["accounturl"].ToString();

            Account = Admin.GetAccountByUrl(url);
        }

        public Account Account { get; private set; }

    }

}