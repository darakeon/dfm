using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers;
using DFM.MVC.Models;
using DFM.Entities;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    public class UserController : Controller
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
                    Services.Safe.SaveUserAndSendVerify(model.User);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e.Message]);
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
                var user = new User();


                try
                {
                    user = Services.Safe.ValidateAndGet(model.Email, model.Password);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e.Message]);
                }


                if (ModelState.IsValid)
                {
                    return user.Active 
                        ? logOnUser(user, returnUrl, model.RememberMe) 
                        : RedirectToAction("SendVerification", new { id = user.Email });
                }
            }

            return View(model);
        }

        private ActionResult logOnUser(User user, String returnUrl = null, Boolean isPersistent = false)
        {
            Authenticate.Set(user.Email, Response, isPersistent);

            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Account");

            return Redirect(returnUrl);
        }

        public ActionResult SendVerification(String id)
        {
            try
            {
                Services.Safe.SendUserVerify(id);
            }
            catch (DFMCoreException e)
            {
                return RedirectToAction("Error", "Ops", new { id = e.Type });
            }

            return View();
        }



        public ActionResult LogOff()
        {
            Authenticate.Clean(Request);

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
                    Services.Safe.SendPasswordReset(model.Email);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e.Message]);
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
