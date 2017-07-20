using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.Email;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Models;
using DFM.Entities;
using DFM.MVC.MultiLanguage;
using DFM.Repositories;

namespace DFM.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly Format formatUserVerification;

        public UserController()
        {
            formatUserVerification = new Format
                {
                    Layout = PlainText.EmailLayout["UserVerification"],
                    Subject = PlainText.Dictionary["UserVerification"],
                };
        }



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
                ModelState.AddModelError("", PlainText.Dictionary["RetypeWrong"]);


            if (ModelState.IsValid)
            {
                try
                {
                    Service.Access.User.SaveAndSendVerify(model.User, formatUserVerification);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
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
                    user = Service.Access.User.ValidateAndGet(model.Email, model.Password);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
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
            var user = Service.Access.User.SelectByEmail(id);

            Service.Access.Security.SendUserVerify(user, formatUserVerification);

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
                    var format = new Format
                        {
                            Layout = PlainText.EmailLayout["PasswordReset"],
                            Subject = PlainText.Dictionary["PasswordReset"],
                        };

                    Service.Access.Security.PasswordReset(model.Email, format);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
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
