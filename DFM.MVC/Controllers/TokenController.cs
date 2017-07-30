using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.Models;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    public class TokenController : Controller
    {
        public ActionResult PasswordReset(String id)
        {
            try
            {
                Services.Safe.TestSecurityToken(id, SecurityAction.PasswordReset);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

            var model = new TokenPasswordResetModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            try
            {
                Services.Safe.TestSecurityToken(id, SecurityAction.PasswordReset);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }


            if (model.Password != model.RetypePassword)
                ModelState.AddModelError("", MultiLanguage.Dictionary["RetypeWrong"]);

            if (ModelState.IsValid)
            {
                try
                {
                    Services.Safe.PasswordReset(id, model.Password);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
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
            try
            {
                Services.Safe.TestSecurityToken(id, SecurityAction.UserVerification);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

            Services.Safe.ActivateUser(id);

            return View();
        }



        public ActionResult Disable(String id)
        {
            try
            {
                Services.Safe.DisableToken(id);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

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
            if (!ModelState.IsValid)
                return View(model);

            model.Token = model.Token.Trim();

            try
            {
                Services.Safe.TestSecurityToken(model.Token, model.SecurityAction);
            }
            catch (DFMCoreException e)
            {
                ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                return View(model);
            }


            switch (model.SecurityAction)
            {
                case SecurityAction.PasswordReset:
                    return RedirectToAction("PasswordReset", new { id = model.Token } );
                case SecurityAction.UserVerification:
                    return RedirectToAction("UserVerification", new { id = model.Token });
                default:
                    ModelState.AddModelError("", MultiLanguage.Dictionary["NotRecognizedAction"]);
                    return View(model);
            }

        }



        private ActionResult invalidTokenAction()
        {
            return View("Invalid");
        }

    }
}
