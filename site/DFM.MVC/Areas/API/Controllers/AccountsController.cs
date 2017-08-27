using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.API.Controllers
{
    [DFMApiAuthorize]
    public class AccountsController : BaseJsonController
    {
        public ActionResult List()
        {
            var model = new AccountsListModel();

            return JsonGet(model);
        }

    }
}
