using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.API.Controllers
{
    [DFMJsonAuthorize]
    public class AccountController : BaseJsonController
    {
        public ActionResult List()
        {
            var model = new AccountListModel();

            return JsonGet(model);
        }

    }
}
