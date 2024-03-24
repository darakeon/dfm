using System;
using DFM.BusinessLogic;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Starters.Routes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class AuthAttribute : Attribute, IAuthorizationFilter
	{
		private readonly AuthParams mandatory;

		public AuthAttribute(AuthParams mandatory = AuthParams.None)
		{
			this.mandatory = mandatory;
		}

		private Service service;
		private Current current => service.Current;
		private ServiceAccess access => service.Access;
		private Boolean isAuthenticated => current.IsAuthenticated;

		private Boolean denyByAdmin    => mandatory.HasFlag(AuthParams.Admin)
											&& !current.IsAdm;

		private Boolean denyByContract => !mandatory.HasFlag(AuthParams.IgnoreContract)
											&& !access.Law.IsLastContractAccepted();
		
		private Boolean denyByTFA      => !mandatory.HasFlag(AuthParams.IgnoreTFA)
											&& !access.Auth.VerifyTicketTFA();

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			service = context.HttpContext.GetService();

			var goAhead = isAuthenticated
							&& !denyByAdmin
							&& !denyByContract
							&& !denyByTFA;

			if (goAhead) return;

			if (!isAuthenticated)
				goToUninvited(context);

			else if (denyByTFA)
				goToTFA(context);

			else if (denyByContract)
				goToContractPage(context);

			else
				goToUninvited(context);
		}

		protected virtual void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "Contract");
		}

		protected virtual void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "TFA");
		}

		protected virtual void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "LogOn");
		}

		protected void goTo(
			AuthorizationFilterContext filterContext,
			[AspMvcController] String controller,
			[AspMvcAction] String action)
		{
			goTo<Default.Main>(filterContext, controller, action);
		}

		protected void goTo<T>(
			AuthorizationFilterContext filterContext,
			[AspMvcController] String controller,
			[AspMvcAction] String action)
			where T : BaseRoute, new()
		{
			var route = new T();
			var area = route.Area;

			filterContext.Result = new RedirectToRouteResult(
				route.Name,
				new { action, controller, area }
			);
		}
	}
}
