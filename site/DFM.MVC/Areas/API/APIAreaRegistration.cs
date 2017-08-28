using System.Web.Mvc;
using DFM.MVC.Areas.API.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API
{
    public class APIAreaRegistration : AreaRegistration
    {
        public override string AreaName => RouteNames.API;

	    public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RouteNames.APILoggedAccount,
                "API-{ticket}/Account-{accounturl}/{controller}/{action}/{id}",
                new { controller = "Moves", action = "List", id = UrlParameter.Optional },
                new[] { typeof(UsersController).Namespace }
            );

            context.MapRoute(
                RouteNames.APILogged,
                "API-{ticket}/{controller}/{action}/{id}",
                new { controller = "Accounts", action = "List", id = UrlParameter.Optional },
                new[] { typeof(UsersController).Namespace }
            );

            context.MapRoute(
                RouteNames.API,
                "API/{controller}/{action}/{id}",
                new { controller = "Users", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UsersController).Namespace }
            );

        }
    }
}
