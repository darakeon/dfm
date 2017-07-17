using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.Core.Exceptions;
using DFM.MVC.Models;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        
        #region LogOn
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
                    user = UserData.ValidateAndGet(model.Email, model.Password);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }


                if (ModelState.IsValid)
                {
                    return logOnUser(user, returnUrl, model.RememberMe);
                }
            }

            return View(model);
        }
        #endregion



        #region SignUp
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
                    UserData.SaveOrUpdate(model.User);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }

                if (ModelState.IsValid)
                {
                    return logOnUser(model.User);
                }
            }

            return View(model);
        }
        #endregion



        public ActionResult LogOff()
        {
            Authenticate.Clean(Request);

            return RedirectToAction("Index", "User");
        }


        private ActionResult logOnUser(User user, String returnUrl = null, Boolean isPersistent = false)
        {
            Authenticate.Set(user.Email, Response, isPersistent);

            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Account");
            
            return Redirect(returnUrl);
        }

    }
}
