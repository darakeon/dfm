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
				RouteNames.DEFAULT,
				"{controller}/{action}/{id}",
				new { controller = "Users", action = "Index", id = UrlParameter.Optional },
				new[] { typeof(UsersController).Namespace }
			);

			routes.MapRoute(
				RouteNames.MOBILE,
				"@{activity}",
				new { controller = "Users", action = "Mobile" },
				new[] { typeof(UsersController).Namespace }
			);
		}
	}
}
