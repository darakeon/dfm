using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
    public class TokenController : BaseController
    {
        public ActionResult Index()
        {
            var model = new TokenIndexModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TokenIndexModel model)
        {
            var errors = model.Test();

            AddErrors(errors);

            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(
                model.SecurityAction.ToString(),
                new { id = model.Token }
            );

        }



        public ActionResult PasswordReset(String id)
        {
            var model = new TokenPasswordResetModel();

            var isValid = model.TestToken(id);

            return isValid
                ? View(model)
                : View("Invalid");
        }

        [HttpPost]
        public ActionResult PasswordReset(String id, TokenPasswordResetModel model)
        {
            var isValid = model.TestToken(id);

            if (!isValid)
                return View("Invalid");

            var errors = model.PasswordReset(id);

            AddErrors(errors);

            return ModelState.IsValid 
                ? View("PasswordResetSuccess") 
                : View(model);
        }



        public ActionResult UserVerification(String id)
        {
            var model = new SafeModel();

            var isValid = model.TestAndActivate(id);

            return isValid
                ? View("UserVerificationSuccess")
                : View("Invalid");
        }



        public ActionResult Disable(String id)
        {
            var model = new SafeModel();

            var isValid = model.Disable(id);

            return isValid
                ? View()
                : View("Invalid");
        }


    }
}
