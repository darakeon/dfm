using System;
using System.Web.Mvc;
using DFM.Core.Database;
using DFM.Core.Enums;
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
                return invalidTokenAction();

            var model = new TokenPasswordResetModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            var action = SecurityData.GetTokenAction(id);

            if (action != SecurityAction.PasswordReset)
                return invalidTokenAction();

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



        public ActionResult UserVerification(String id)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            var action = SecurityData.GetTokenAction(id);

            if (action != SecurityAction.UserVerification)
                return invalidTokenAction();

            SecurityData.UserActivate(id);

            return View();
        }



        public ActionResult Deactivate(String id)
        {
            var tokenExist = SecurityData.TokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            SecurityData.Deactivate(id);

            return View();
        }



        public ActionResult Received()
        {
            var model = new TokenReceivedModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Received(TokenReceivedModel model)
        {
            model.Token = model.Token.Trim();

            var exists = SecurityData.TokenExist(model.Token);

            if (!exists)
                return invalidTokenAction();


            var action = SecurityData.GetTokenAction(model.Token);

            switch (action)
            {
                case SecurityAction.PasswordReset:
                    return RedirectToAction("PasswordReset", new { id = model.Token } );
                case SecurityAction.UserVerification:
                    return RedirectToAction("UserVerification", new { id = model.Token });
                default:
                    ModelState.AddModelError("", PlainText.Dictionary["NotRecognizedAction"]);
                    return View(model);
            }

        }



        private ActionResult invalidTokenAction()
        {
            return View("Invalid");
        }

    }
}
