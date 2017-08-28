using System;
using System.Web;
using System.Web.Mvc;

namespace DFM.MVC.Helpers.Authorize
{
	public class DFMAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly Boolean admin;

		public DFMAuthorizeAttribute(Boolean admin = false)
		{
			this.admin = admin;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return Service.Current.IsAuthenticated
				&& (!admin || Service.Current.IsAdm);
		}
	}
}