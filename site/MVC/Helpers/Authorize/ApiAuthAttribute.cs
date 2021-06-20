using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class ApiAuthAttribute : AuthAttribute
	{
		public ApiAuthAttribute(AuthParams mandatory = AuthParams.None)
			: base(mandatory | AuthParams.Mobile) { }

		protected override void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo<Apis.Main>(filterContext, "Users", "AcceptOnlineContract");
		}

		protected override void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo<Apis.Main>(filterContext, "Users", "Uninvited");
		}

		protected override void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo<Apis.Main>(filterContext, "Users", "OpenTFA");
		}
	}
}
