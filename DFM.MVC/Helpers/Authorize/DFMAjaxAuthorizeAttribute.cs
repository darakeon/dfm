using System.Web;
using Ak.MVC.Authentication;

namespace DFM.MVC.Helpers.Authorize
{
    public class DFMAjaxAuthorizeAttribute : AjaxAuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Auth.Current.IsAuthenticated;
        }
    }
}