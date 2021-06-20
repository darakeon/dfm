using System;
using DFM.BusinessLogic;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Starters.Routes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.MVC.Helpers.Authorize
{
	public class AuthAttribute : Attribute, IAuthorizationFilter
	{
		private readonly Boolean needAdmin;
		private readonly Boolean ignoreContract;
		private readonly Boolean ignoreTFA;
		private readonly Boolean isMobile;

		public AuthAttribute(
			Boolean needAdmin = false,
			Boolean ignoreContract = false,
			Boolean ignoreTFA = false,
			Boolean isMobile = false
		)
		{
			this.ignoreContract = ignoreContract;
			this.needAdmin = needAdmin;
			this.ignoreTFA = ignoreTFA;
			this.isMobile = isMobile;
		}

		private Service service;
		private Current current => service.Current;
		private ServiceAccess access => service.Access;
		private Boolean isAuthenticated => current.IsAuthenticated;
		private Boolean denyByAdmin => needAdmin && !current.IsAdm;
		private Boolean denyByContract => !ignoreContract && !access.Safe.IsLastContractAccepted();
		private Boolean denyByTFA => !ignoreTFA && !access.Safe.VerifyTicketTFA();
		private Boolean denyByMobile => isMobile && !access.Safe.VerifyTicketType(TicketType.Mobile);

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			service = context.HttpContext.GetService();

			var goAhead = isAuthenticated
			              && !denyByAdmin
			              && !denyByContract
			              && !denyByTFA
			              && !denyByMobile;

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
