using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.MVC.Models;
using DFM.MVC.MultiLanguage;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    public class TokenController : Controller
    {
        public ActionResult PasswordReset(String id)
        {
            var tokenExist =  Services.Safe.SecurityTokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            var model = new TokenPasswordResetModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            var tokenExist =  Services.Safe.SecurityTokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            var action =  Services.Safe.GetSecurityTokenAction(id);

            if (action != SecurityAction.PasswordReset)
                return invalidTokenAction();

            if (model.Password != model.RetypePassword)
                ModelState.AddModelError("", PlainText.Dictionary["RetypeWrong"]);

            if (ModelState.IsValid)
            {
                try
                {
                    Services.Safe.PasswordReset(id, model.Password);
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
            var tokenExist =  Services.Safe.SecurityTokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            var action =  Services.Safe.GetSecurityTokenAction(id);

            if (action != SecurityAction.UserVerification)
                return invalidTokenAction();

            Services.Safe.ActivateUser(id);

            return View();
        }



        public ActionResult Deactivate(String id)
        {
            var tokenExist =  Services.Safe.SecurityTokenExist(id);

            if (!tokenExist)
                return invalidTokenAction();

            Services.Safe.Deactivate(id);

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

            var exists =  Services.Safe.SecurityTokenExist(model.Token);

            if (!exists)
                return invalidTokenAction();


            var action =  Services.Safe.GetSecurityTokenAction(model.Token);

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
