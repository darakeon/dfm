using System.Web;
using System.Web.Mvc;

namespace DFM.MVC.Helpers.Authorize
{
    public class DFMAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Auth.Current.IsAuthenticated;
        }
    }
}