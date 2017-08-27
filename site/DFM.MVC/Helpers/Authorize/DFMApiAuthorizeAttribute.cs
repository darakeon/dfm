using System.Web.Mvc;
using System.Web.Routing;

namespace DFM.MVC.Helpers.Authorize
{
    public class DFMApiAuthorizeAttribute : DFMAuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var route = new RouteValueDictionary(new { controller = "Users", action = "Uninvited" });

            filterContext.Result = new RedirectToRouteResult(RouteNames.API, route);
        }

    }
}