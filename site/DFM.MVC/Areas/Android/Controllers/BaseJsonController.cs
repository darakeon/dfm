using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Android.Controllers
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
