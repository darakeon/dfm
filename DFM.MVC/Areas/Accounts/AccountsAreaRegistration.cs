using System;
using System.Web.Mvc;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Accounts
{
    public class AccountsAreaRegistration : AreaRegistration
    {
        public override String AreaName
        {
            get
            {
                return "Accounts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RouteNames.Accounts,
                "Accounts/{accounturl}/{controller}/{action}/{id}",
                new { controller = "Report", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
