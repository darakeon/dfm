using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class JsonAuthAttribute : AuthAttribute
	{
		public JsonAuthAttribute(Boolean needAdmin = false)
			: base(needAdmin) { }

		protected override void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "Reload");
		}

		protected override void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "Reload");
		}

		protected override void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "Reload");
		}
	}
}
