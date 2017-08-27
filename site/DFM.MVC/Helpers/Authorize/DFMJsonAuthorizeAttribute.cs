using System.Web.Mvc;
using System.Web.Routing;

namespace DFM.MVC.Helpers.Authorize
{
    public class DFMJsonAuthorizeAttribute : DFMAuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var route = new RouteValueDictionary(new { controller = "User", action = "Uninvited" });

            filterContext.Result = new RedirectToRouteResult(RouteNames.Android, route);
        }

    }
}