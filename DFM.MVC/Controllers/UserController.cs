using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.Entities;
using DFM.Repositories;

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
                    Services.Safe.SaveUserAndSendVerify(model.User.Email, model.User.Password);
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
                var user = new User();


                try
                {
                    user = Current.Set(model.Email, model.Password, Response, model.RememberMe);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }


                if (ModelState.IsValid)
                {
                    return user.Active 
                        ? String.IsNullOrEmpty(returnUrl)
                            ? (ActionResult) RedirectToAction("Index", "Account")
                            : Redirect(returnUrl)
                        : RedirectToAction("SendVerification", new { id = user.Email });
                }
            }

            return View(model);
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
            Current.Clean(Request);

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
