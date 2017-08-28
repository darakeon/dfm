using System;
using System.Web.Mvc;
using DK.Generic.Collection;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    public class OpsController : Controller
    {
        public ActionResult Index()
        {
            var model = new OpsModel();

            return View(model);
        }

        public ActionResult Code(Int32 id)
        {
            if (!id.IsIn(404, 500))
                return RedirectToAction("Index");


            var model = new OpsCodeModel();

            if (id == 500)
                model.EmailSent = ErrorManager.EmailSent;


            return View(id.ToString(), model);
        }


        public ActionResult Error(ExceptionPossibilities id)
        {
            var model = new OpsModel(id);

            return View(model);
        }


    }
}
