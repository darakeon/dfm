using System.Web.Mvc;
using DFM.MVC.Areas.Json.Controllers;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Json
{
    public class JsonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return RouteNames.Json;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RouteNames.Json,
                "Json/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );
        }
    }
}
