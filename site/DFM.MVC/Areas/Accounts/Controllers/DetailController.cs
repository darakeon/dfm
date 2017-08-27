using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Accounts.Models;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class DetailController : Controller
    {
        public ActionResult Add(Int32 position = 0)
        {
            var model = new MoveAddDetailModel(position);

            return View(model);
        }


    }
}
