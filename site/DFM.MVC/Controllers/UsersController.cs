using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	public class UsersController : BaseController
	{
		public ActionResult Index()
		{
			return BaseModelView();
		}



		public ActionResult SignUp()
		{
			var model = new UsersSignUpModel();

			return View(model);
		}

		[HttpPost]
		public ActionResult SignUp(UsersSignUpModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.ValidateAndSendVerify(ModelState);

				AddErrors(errors);
			}

			return ModelState.IsValid
				? View("SignUpSuccess")
				: View(model);
		}



		public ActionResult LogOn(String returnUrl)
		{
			var model = new UsersLogOnModel();

			return View(model);
		}

		[HttpPost]
		public ActionResult LogOn(UsersLogOnModel model, String returnUrl)
		{
			var logOnError = model.TryLogOn();

			if (logOnError == null)
			{
				return String.IsNullOrEmpty(returnUrl)
					? (ActionResult) RedirectToAction("Index", "Accounts")
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

			return RedirectToAction("Index", "Users");
		}



		public ActionResult ForgotPassword()
		{
			var model = new UsersForgotPasswordModel();

			return View(model);
		}

		[HttpPost]
		public ActionResult ForgotPassword(UsersForgotPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.SendPasswordReset();

				AddErrors(errors);
			}

			return ModelState.IsValid
				? View("ForgotPasswordSuccess")
				: View(model);
		}



		[DFMAuthorize]
		public ActionResult Config()
		{
			return View(new UsersConfigModel());
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ChangeConfig(UsersConfigModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.Main.Save();

				AddErrors(errors);
			}

			if (!ModelState.IsValid)
				return View("Config", model);

			return RedirectToAction("Index", "Accounts");
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ChangePassword(UsersConfigModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.Info.ChangePassword();

				AddErrors(errors);
			}

			if (!ModelState.IsValid)
				return View("Config", model);

			return RedirectToAction("Index", "Accounts");
		}

		[DFMAuthorize, HttpPost]
		public ActionResult UpdateEmail(UsersConfigModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.Info.UpdateEmail();

				AddErrors(errors);
			}

			if (!ModelState.IsValid)
				return View("Config", model);

			return RedirectToAction("Index", "Accounts");
		}

	}
}