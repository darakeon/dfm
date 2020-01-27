using System;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class ApiAuthAttribute : AuthAttribute
	{
		public ApiAuthAttribute(Boolean needTFA = true)
			: base(needTFA: needTFA, isMobile: true) { }

		protected override void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo<RouteApi>(filterContext, "Users", "AcceptOnlineContract");
		}

		protected override void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo<RouteApi>(filterContext, "Users", "Uninvited");
		}

		protected override void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo<RouteApi>(filterContext, "Users", "OpenTFA");
		}
	}
}
