using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using DFM.Generic;

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
				? BaseModelView("SignUpSuccess")
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
				return BaseModelView("SendVerification");
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
				? BaseModelView("ForgotPasswordSuccess")
				: View(model);
		}



		[DFMAuthorize]
		public ActionResult Config()
		{
			return View(new UsersConfigModel());
		}

		private ActionResult config(UsersConfigModel model, Func<IList<String>> save)
		{
			if (ModelState.IsValid)
			{
				var errors = save();
				AddErrors(errors);
			}

			if (!ModelState.IsValid)
			{
				return View("Config", model);
			}

			return RedirectToAction("Index", "Accounts");

		}

		[DFMAuthorize, HttpPost]
		public ActionResult ConfigOptions(UsersConfigModel model)
		{
			return config(model, () => model.Main.Save());
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ConfigPassword(UsersConfigModel model)
		{
			return config(model, () => model.Info.ChangePassword());
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ConfigEmail(UsersConfigModel model)
		{
			return config(model, () => model.Info.UpdateEmail());
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ConfigTheme(UsersConfigModel model)
		{
			return config(model, () => model.Theme.Change());
		}

		[DFMAuthorize, HttpPost]
		public ActionResult ConfigTFA(UsersConfigModel model)
		{
			return config(model, () => model.TFA.Activate());
		}



		[DFMAuthorize(needContract: false)]
		public ActionResult Contract()
		{
			var model = new UsersContractModel();
			return View(model);
		}

		[DFMAuthorize(needContract: false), HttpPost]
		public ActionResult Contract(UsersContractModel model)
		{
			model.AcceptContract(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		public ActionResult Mobile()
		{
			return Redirect(Cfg.GooglePlay);
		}

		[DFMAuthorize]
		public ActionResult EndWizard()
		{
			var model = new UsersEndWizard();
			return View(model);
		}

		[DFMAuthorize(needContract: false, needTFA: false)]
		public ActionResult TFA()
		{
			var model = new UsersTFAModel();
			return View(model);
		}

		[DFMAuthorize(needContract: false, needTFA: false), HttpPost]
		public ActionResult TFA(UsersTFAModel model)
		{
			model.Validate(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}
	}
}