using System;
using System.Web.Mvc;
using DFM.Core.Database;
using DFM.Core.Exceptions;
using DFM.MVC.Models;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Controllers
{
    public class TokenController : Controller
    {
        public ActionResult PasswordReset(String id)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return View("Deactivated");

            var model = new TokenPasswordResetModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return View("Deactivated");

            if (model.Password != model.RetypePassword)
                ModelState.AddModelError("", PlainText.Dictionary["RetypeWrong"]);

            if (ModelState.IsValid)
            {
                try
                {
                    SecurityData.PasswordReset(id, model.Password);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }

                if (ModelState.IsValid)
                {
                    return View("PasswordResetSuccess");
                }
            }

            return View(model);
        }



        public ActionResult Deactivate(String id)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return View("Deactivated");

            SecurityData.Deactivate(id);

            return View();
        }

    }
}
