using System;
using System.Web.Mvc;
using Ak.MVC.Authentication;
using DFM.Core.Helpers;
using DFM.MVC.Helpers;
using DFM.MVC.Models;
using DFM.Core.Database;
using DFM.Core.Entities;

namespace DFM.MVC.Controllers
{
    public class UserController : Controller
    {
        readonly UserData userData = new UserData();



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
                    user = userData.Validate(model.Login, model.Password);
                }
                catch (CoreValidationException e)
                {
                    ModelState.AddModelError("", e.Message);
                }


                if (ModelState.IsValid)
                {
                    return LogOnUser(user, returnUrl);
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
            if (ModelState.IsValid)
            {
                try
                {
                    userData.SaveOrUpdate(model.User);
                }
                catch (CoreValidationException e)
                {
                    ModelState.AddModelError("", e.Message);
                }

                if (ModelState.IsValid)
                {
                    return LogOnUser(model.User);
                }
            }

            return View(model);
        }
        #endregion



        public ActionResult LogOff()
        {
            Authenticate.Clean(Request);

            return RedirectToRoute(RouteNames.Default);
        }


        private ActionResult LogOnUser(User user, String returnUrl = null)
        {
            Authenticate.Set(user.Login, Response);

            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Account");
            
            return Redirect(returnUrl);
        }

    }
}
