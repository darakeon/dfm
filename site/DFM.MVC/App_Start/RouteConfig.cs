using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                RouteNames.Default,
                "{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );
        }
    }
}
