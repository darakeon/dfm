using System;
using System.Web.Mvc;
using Ak.Generic.Collection;
using DFM.Core.Helpers;
using DFM.MVC.Authentication;
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
            {
                model.EmailSent = SendErrorEmail();
            }


            if (Current.IsAuthenticated)
                return View(id.ToString(), model);
            
            // ReSharper disable Asp.NotResolved
            return View(id.ToString(), model);
            // ReSharper restore Asp.NotResolved
        }

        private Boolean SendErrorEmail()
        {
            var body = String.Format("({0}) Look at Elmah, something is going wrong.", DateTime.Now);

            try
            {
                new EmailSender()
                    .ToDefault()
                    .Subject("Shit!!!")
                    .Body(body)
                    .Send();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
