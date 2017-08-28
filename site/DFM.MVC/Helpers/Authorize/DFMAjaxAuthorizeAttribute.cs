using System.Web;
using DK.MVC.Authentication;

namespace DFM.MVC.Helpers.Authorize
{
	public class DFMAjaxAuthorizeAttribute : AjaxAuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return Service.Current.IsAuthenticated;
		}
	}
}