using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    [DFMAuthorize]
    public class LoginsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new LoginsIndexModel());
        }

        public ActionResult Delete(String id)
        {
            var model = new SafeModel();

            model.DisableLogin(id);

            return RedirectToAction("Index");
        }



    }
}
