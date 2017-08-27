using System.Web.Mvc;
using DFM.MVC.Areas.Android.Models;

namespace DFM.MVC.Areas.Android.Controllers
{
    public class AccountController : BaseJsonController
    {
        public ActionResult List()
        {
            var model = new AccountListModel();

            return JsonGet(model);
        }

    }
}
