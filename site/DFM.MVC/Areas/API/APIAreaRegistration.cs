using System.Web.Mvc;
using DFM.MVC.Areas.API.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API
{
	public class ApiAreaRegistration : AreaRegistration
	{
		public override string AreaName => RouteNames.API;

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				RouteNames.API_ACCOUNT,
				"api/account-{accountUrl}/{controller}/{action}/{id}",
				new { controller = "Moves", action = "List", id = UrlParameter.Optional },
				new[] { typeof(UsersController).Namespace }
			);

			context.MapRoute(
				RouteNames.API,
				"api/{controller}/{action}/{id}",
				new { controller = "Status", action = "Index", id = UrlParameter.Optional },
				new[] { typeof(StatusController).Namespace }
			);
		}
	}
}
