using System.Web.Routing;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.MVC.Helpers.Controllers;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class BaseAccountsController : BaseController
    {
        protected Account Account;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!Current.IsAuthenticated)
                return;

            var url = RouteData.Values["accounturl"].ToString();

            Account = Services.Admin.SelectAccountByUrl(url);
        }

    }
}
