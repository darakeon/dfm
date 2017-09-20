using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Helpers.Authorize
{
	public class DFMApiAuthorizeAttribute : DFMAuthorizeAttribute
	{
		protected override void GoToContractPage(AuthorizationContext filterContext)
		{
			var route = new RouteValueDictionary(new { controller = "Users", action = "AcceptOnlineContract" });
			filterContext.Result = new RedirectToRouteResult(RouteNames.API, route);
		}

		protected override void GoToUninvited(AuthorizationContext filterContext)
		{
			var route = new RouteValueDictionary(new { controller = "Users", action = "Uninvited" });
			filterContext.Result = new RedirectToRouteResult(RouteNames.API, route);
		}
	}
}