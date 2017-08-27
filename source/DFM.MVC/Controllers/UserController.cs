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
            var errors = model.ValidateAndSendVerify(ModelState);

            AddErrors(errors);

            return ModelState.IsValid
                ? View("SignUpSuccess")
                : View(model);
        }



        public ActionResult LogOn(String returnUrl)
        {
            var model = new UserLogOnModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult LogOn(UserLogOnModel model, String returnUrl)
        {
            var logOnError = model.LogOn();

            if (logOnError == null)
            {
                return String.IsNullOrEmpty(returnUrl)
                    ? (ActionResult)RedirectToAction("Index", "Account")
                    : Redirect(returnUrl);
            }

            if (logOnError.Type == ExceptionPossibilities.DisabledUser)
            {
                return View("SendVerification");
            }

            ModelState.AddModelError("", MultiLanguage.Dictionary[logOnError]);

            return View(model);
        }



        public ActionResult LogOff()
        {
            var model = new SafeModel();

            model.LogOff();

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
            var errors = model.SendPasswordReset();

            AddErrors(errors);

            return ModelState.IsValid
                ? View("ForgotPasswordSuccess")
                : View(model);
        }



    }
}
