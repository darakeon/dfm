using System;
using System.Web;
using System.Web.Mvc;
using DFM.MVC.Models;

namespace DFM.MVC.Helpers.Authorize
{
	public class DFMAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly Boolean needAdmin;
		private readonly Boolean needContract;
		private readonly Boolean needTFA;

		public DFMAuthorizeAttribute(
			Boolean needAdmin = false,
			Boolean needContract = true,
			Boolean needTFA = true
		)
		{
			this.needContract = needContract;
			this.needAdmin = needAdmin;
			this.needTFA = needTFA;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return isAuthenticated && !denyByAdmin && !denyByContract && !denyByTFA;
		}

		private Boolean isAuthenticated => Service.Current.IsAuthenticated;
		private Boolean denyByAdmin => needAdmin && !Service.Current.IsAdm;
		private Boolean denyByContract => needContract && !Service.Access.Safe.IsLastContractAccepted();
		private Boolean denyByTFA => needTFA && !Service.Access.Safe.VerifyTicket();

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (isAuthenticated)
			{
				if (denyByContract)
				{
					GoToContractPage(filterContext);
					return;
				}

				if (denyByTFA)
				{
					GoToTFA(filterContext);
					return;
				}
			}

			GoToUninvited(filterContext);
		}

		protected virtual void GoToContractPage(AuthorizationContext filterContext)
		{
			var url = BaseSiteModel.Url.Action("Contract", "Users");
			filterContext.Result = new RedirectResult(url);
		}

		protected virtual void GoToTFA(AuthorizationContext filterContext)
		{
			var url = BaseSiteModel.Url.Action("TFA", "Users");
			filterContext.Result = new RedirectResult(url);
		}

		protected virtual void GoToUninvited(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
		}
	}
}