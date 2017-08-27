using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        
        public ActionResult SignUp()
        {
            var model = new UserSignUpModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(UserSignUpModel model)
        {
            if (model.Password != model.RetypePassword)
                ModelState.AddModelError("", MultiLanguage.Dictionary["RetypeWrong"]);


            if (ModelState.IsValid)
            {
                try
                {
                    model.SaveUserAndSendVerify();
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }

                if (ModelState.IsValid)
                {
                    return View("SignUpSuccess");
                }
            }

            return View(model);
        }



        public ActionResult LogOn(String returnUrl)
        {
            var model = new UserLogOnModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult LogOn(UserLogOnModel model, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Current.Set(model.Email, model.Password);
                }
                catch (DFMCoreException e)
                {
                    if (e.Type == ExceptionPossibilities.DisabledUser)
                        return RedirectToAction("SendVerification", new {id = model.Email});

                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }


                if (ModelState.IsValid)
                {
                    return String.IsNullOrEmpty(returnUrl)
                        ? (ActionResult) RedirectToAction("Index", "Account")
                        : Redirect(returnUrl);
                }
            }

            return View(model);
        }



        public ActionResult SendVerification(String id)
        {
            var model = new SafeModel();

            try
            {
                model.SendUserVerify(id);
            }
            catch (DFMCoreException e)
            {
                return RedirectToAction("Error", "Ops", new { id = e.Type });
            }

            return View();
        }



        public ActionResult LogOff()
        {
            Current.Clean();

            return RedirectToAction("Index", "User");
        }

        

        public ActionResult ForgotPassword()
        {
            var model = new UserForgotPasswordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.SendPasswordReset();
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }

                if (ModelState.IsValid)
                {
                    return View("ForgotPasswordSuccess");
                }
            }

            return View(model);
        }



    }
}
