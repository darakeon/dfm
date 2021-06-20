using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class JsonAuthAttribute : AuthAttribute
	{
		public JsonAuthAttribute(AuthParams mandatory = AuthParams.None)
			: base(mandatory) { }

		protected override void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Generic", "Reload");
		}

		protected override void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Generic", "Reload");
		}

		protected override void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Generic", "Reload");
		}
	}
}
