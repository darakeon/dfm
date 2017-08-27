using System.Web.Mvc;
using DFM.MVC.Areas.API.Controllers;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.API
{
    public class APIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return RouteNames.API;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RouteNames.API,
                "API/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );

            context.MapRoute(
                RouteNames.APILogged,
                "API-{ticket}/{controller}/{action}/{id}",
                new { controller = "Account", action = "List", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );

            context.MapRoute(
                RouteNames.APILoggedAccount,
                "API-{ticket}/Account-{accountname}/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );

        }
    }
}
