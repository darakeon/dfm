using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [DFMAuthorize]
    public class DetailController : Controller
    {
        public ActionResult Add(Int32 position = 0)
        {
            var model = new MoveAddDetailModel(position);

            return View(model);
        }


    }
}
