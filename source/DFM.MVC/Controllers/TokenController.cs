using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    public class TokenController : Controller
    {
        public ActionResult Index()
        {
            var model = new TokenIndexModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TokenIndexModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Token = model.Token.Trim();

            try
            {
                model.Test();
            }
            catch (DFMCoreException e)
            {
                ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                return View(model);
            }


            switch (model.SecurityAction)
            {
                case SecurityAction.PasswordReset:
                    return RedirectToAction("PasswordReset", new { id = model.Token });
                case SecurityAction.UserVerification:
                    return RedirectToAction("UserVerification", new { id = model.Token });
                default:
                    ModelState.AddModelError("", MultiLanguage.Dictionary["NotRecognizedAction"]);
                    return View(model);
            }

        }



        public ActionResult PasswordReset(String id)
        {
            var model = new TokenPasswordResetModel();

            try
            {
                model.TestSecurityToken(id);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            try
            {
                model.TestSecurityToken(id);
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
                    model.PasswordReset(id);
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
            var model = new SafeModel();

            try
            {
                model.TestUserVerificationToken(id);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

            model.ActivateUser(id);

            return View("UserVerificationSuccess");
        }



        public ActionResult Disable(String id)
        {
            var model = new SafeModel();

            try
            {
                model.Disable(id);
            }
            catch (DFMCoreException)
            {
                return invalidTokenAction();
            }

            return View();
        }




        private ActionResult invalidTokenAction()
        {
            return View("Invalid");
        }


    }
}
