using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Json.Controllers
{
    public class BaseJsonController : BaseController
    {
        protected JsonResult JsonGet(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonGetError(object data)
        {
            return JsonGet(new { error = data });
        }

    }
}
