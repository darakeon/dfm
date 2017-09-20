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

		public DFMAuthorizeAttribute(Boolean needAdmin = false, Boolean needContract = true)
		{
			this.needContract = needContract;
			this.needAdmin = needAdmin;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return isAuthenticated && !denyByAdmin && !denyByContract;
		}

		private Boolean isAuthenticated => Service.Current.IsAuthenticated;
		private Boolean denyByAdmin => needAdmin && !Service.Current.IsAdm;
		private Boolean denyByContract => needContract && !Service.Access.Safe.IsLastContractAccepted();

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (isAuthenticated && denyByContract)
			{
				GoToContractPage(filterContext);
			}
			else
			{
				GoToUninvited(filterContext);
			}
		}

		protected virtual void GoToContractPage(AuthorizationContext filterContext)
		{
			var url = BaseModel.Url.Action("Contract", "Users");
			filterContext.Result = new RedirectResult(url);
		}

		protected virtual void GoToUninvited(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
		}
	}
}