using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Helpers.Authorize
{
	public class DFMApiAuthorizeAttribute : DFMAuthorizeAttribute
	{
		public DFMApiAuthorizeAttribute(Boolean needTFA = true) : base(needTFA: needTFA) { }

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

		protected override void GoToTFA(AuthorizationContext filterContext)
		{
			var route = new RouteValueDictionary(new { controller = "Users", action = "OpenTFA" });
			filterContext.Result = new RedirectToRouteResult(RouteNames.API, route);
		}
	}
}