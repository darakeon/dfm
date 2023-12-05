using System;
using DFM.API.Helpers.Extensions;
using DFM.API.Starters.Routes;
using DFM.BusinessLogic;
using DFM.Entities.Enums;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.API.Helpers.Authorize
{
	public class AuthAttribute : Attribute, IAuthorizationFilter
	{
		private readonly AuthParams mandatory;

		public AuthAttribute(AuthParams mandatory = AuthParams.None)
		{
			this.mandatory = mandatory | AuthParams.Mobile;
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
		
		private Boolean denyByMobile   => !access.Auth.VerifyTicketType(TicketType.Mobile);

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
				goToLoginRequested(context);

			else if (denyByTFA)
				goToTFA(context);

			else if (denyByContract)
				goToContractPage(context);

			else
				goToUninvited(context);
		}

		protected virtual void goToContractPage(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "AcceptOnlineContract");
		}

		protected virtual void goToTFA(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "OpenTFA");
		}

		protected virtual void goToLoginRequested(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "LoginRequested");
		}

		protected virtual void goToUninvited(AuthorizationFilterContext filterContext)
		{
			goTo(filterContext, "Users", "Uninvited");
		}

		protected void goTo(
			AuthorizationFilterContext filterContext,
			[AspMvcController] String controller,
			[AspMvcAction] String action)
		{
			var route = new Apis.Main();
			var area = route.Area;

			filterContext.Result = new RedirectToRouteResult(
				route.Name,
				new { action, controller, area }
			);
		}
	}
}
