using System.Web.Mvc;
using DFM.MVC.Areas.Android.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Android.Controllers
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
