using System.Web.Mvc;
using DFM.MVC.Areas.Android.Controllers;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Android
{
    public class JsonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return RouteNames.Android;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RouteNames.Android,
                "Android/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );

            context.MapRoute(
                RouteNames.AndroidLogged,
                "Android-{ticket}/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { typeof(UserController).Namespace }
            );
        }
    }
}
