using System;
using System.Web.Mvc;
using Ak.Generic.Collection;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    public class OpsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Code(Int32 id)
        {
            if (!id.IsIn(400, 500))
                return RedirectToAction("Index");


            var model = new OpsCodeModel();

            if (id == 500)
                model.EmailSent = ErrorManager.EmailSent;


            // ReSharper disable Asp.NotResolved
            return View(id.ToString(), model);
            // ReSharper restore Asp.NotResolved
        }


        public ActionResult Error(ExceptionPossibilities id)
        {
            return View(id);
        }


    }
}
