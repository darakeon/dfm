using System.Web.Mvc;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Helpers.Authorize
{
	public class JsonAuthAttribute : AuthAttribute
	{
		public JsonAuthAttribute() : base(order: 1) { }

		protected override void goToContractPage(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Default, "Users", "Reload");
		}

		protected override void goToUninvited(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Default, "Users", "Reload");
		}

		protected override void goToTFA(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Default, "Users", "Reload");
		}
	}
}
