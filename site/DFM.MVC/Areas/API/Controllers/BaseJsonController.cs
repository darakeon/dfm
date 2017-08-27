using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Controllers
{
    public class BaseJsonController : BaseController
    {
        protected JsonResult JsonGet(object result)
        {
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonGetError(String result)
        {
            return Json(new { error = result }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonPostSuccess()
        {
            return JsonPost( new { success = true });
        }

        protected JsonResult JsonPost(object result)
        {
            return Json(new { data = result });
        }

        protected JsonResult JsonPostError(String result)
        {
            return Json(new { error = result });
        }


    }
}
