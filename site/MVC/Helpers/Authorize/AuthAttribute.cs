using System;
using System.Web;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Authorize
{
	public class AuthAttribute : AuthorizeAttribute
	{
		private readonly Boolean needAdmin;
		private readonly Boolean needContract;
		private readonly Boolean needTFA;
		private readonly Boolean isMobile;

		public AuthAttribute(
			Boolean needAdmin = false,
			Boolean needContract = true,
			Boolean needTFA = true,
			Boolean isMobile = false
		)
		{
			this.needContract = needContract;
			this.needAdmin = needAdmin;
			this.needTFA = needTFA;
			this.isMobile = isMobile;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return isAuthenticated 
			       && !denyByAdmin 
			       && !denyByContract 
			       && !denyByTFA
			       && !denyByMobile;
		}

		private Boolean isAuthenticated => Service.Current.IsAuthenticated;
		private Boolean denyByAdmin => needAdmin && !Service.Current.IsAdm;
		private Boolean denyByContract => needContract && !Service.Access.Safe.IsLastContractAccepted();
		private Boolean denyByTFA => needTFA && !Service.Access.Safe.VerifyTicket();
		private Boolean denyByMobile => isMobile && !Service.Access.Safe.VerifyTicket(TicketType.Mobile);

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (isAuthenticated)
			{
				if (denyByTFA)
				{
					goToTFA(filterContext);
					return;
				}

				if (denyByContract)
				{
					goToContractPage(filterContext);
					return;
				}
			}

			goToUninvited(filterContext);
		}

		protected virtual void goToContractPage(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Default, "Users", "Contract");
		}

		protected virtual void goToTFA(AuthorizationContext filterContext)
		{
			goTo(filterContext, RouteNames.Default, "Users", "TFA");
		}

		protected void goTo(
			AuthorizationContext filterContext,
			String routeName,
			[AspMvcController] String controller,
			[AspMvcAction] String action)
		{
			var url = BaseSiteModel.Url.RouteUrl(
				routeName,
				new { action, controller }
			);

			filterContext.Result = new RedirectResult(url);
		}

		protected virtual void goToUninvited(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
		}
	}
}
