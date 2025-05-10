using System;
using DFM.BaseWeb.Authorize;
using DFM.BaseWeb.Routes;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Routes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Authorize
{
	public class AuthAttribute : BaseAuthAttribute
	{
		public AuthAttribute(AuthParams mandatory = AuthParams.None)
			: base(mandatory) { }

		protected override void makeResult(AuthorizationFilterContext context, Error error)
		{
			switch (error)
			{
				case Error.LoginRequested:
					goToUninvited(context);
					break;

				case Error.TFANotVerified:
					goToTFA(context);
					break;

				case Error.NotSignedLastContract:
					goToContractPage(context);
					break;

				default:
					goToUninvited(context);
					break;
			}
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
