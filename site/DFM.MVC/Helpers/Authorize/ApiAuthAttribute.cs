using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Helpers.Authorize
{
	public class ApiAuthAttribute : AuthAttribute
	{
		public ApiAuthAttribute(Boolean needTFA = true)
			: base(needTFA: needTFA, isMobile: true) { }

		protected override void goToContractPage(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Api, "Users", "AcceptOnlineContract");
		}

		protected override void goToUninvited(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Api, "Users", "Uninvited");
		}

		protected override void goToTFA(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Api, "Users", "OpenTFA");
		}
	}
}
