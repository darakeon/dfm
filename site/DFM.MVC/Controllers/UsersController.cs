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
		[HttpGet]
		public ActionResult Index()
		{
			return BaseModelView();
		}

		[HttpGet]
		public ActionResult SignUp()
		{
			var model = new UsersSignUpModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
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

		[HttpGet]
		public ActionResult LogOn(String returnUrl)
		{
			var model = new UsersLogOnModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult LogOn(UsersLogOnModel model, String returnUrl)
		{
			var logOnError = model.TryLogOn();

			if (logOnError == null)
			{
				return String.IsNullOrEmpty(returnUrl)
					? (ActionResult) RedirectToAction("Index", "Accounts")
					: Redirect(returnUrl);
			}

			if (logOnError.Type == DfMError.DisabledUser)
			{
				return BaseModelView("SendVerification");
			}

			ModelState.AddModelError("", MultiLanguage.Dictionary[logOnError]);

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			var model = new SafeModel();

			model.LogOff();

			return RedirectToAction("Index", "Users");
		}

		[HttpGet]
		public ActionResult ForgotPassword()
		{
			var model = new UsersForgotPasswordModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
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

		[HttpGet, DFMAuthorize]
		public ActionResult Config()
		{
			return View(new UsersConfigModel());
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult ConfigOptions(UsersConfigModel model)
		{
			return config(model, () => model.Main.Save());
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult ConfigPassword(UsersConfigModel model)
		{
			return config(model, () => model.Info.ChangePassword());
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult ConfigEmail(UsersConfigModel model)
		{
			return config(model, () => model.Info.UpdateEmail());
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult ConfigTheme(UsersConfigModel model)
		{
			return config(model, () => model.ThemeOpt.Change());
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult ConfigTFA(UsersConfigModel model)
		{
			return config(model, () => model.TFA.Activate());
		}

		private ActionResult config(UsersConfigModel model, Func<IList<String>> save)
		{
			if (ModelState.IsValid)
				AddErrors(save());

			if (ModelState.IsValid)
				return RedirectToAction("Index", "Accounts");

			return View("Config", model);
		}

		[HttpGet]
		public ActionResult Contract()
		{
			var model = new UsersContractModel();
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize(needContract: false)]
		public ActionResult Contract(UsersContractModel model)
		{
			model.AcceptContract(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[HttpGet]
		public ActionResult Mobile()
		{
			return Redirect(Cfg.GooglePlay);
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult EndWizard()
		{
			var model = new UsersEndWizard();
			return View(model);
		}

		[HttpGet, DFMAuthorize(needContract: false, needTFA: false)]
		public ActionResult TFA()
		{
			var model = new UsersTFAModel();
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize(needContract: false, needTFA: false)]
		public ActionResult TFA(UsersTFAModel model)
		{
			model.Validate(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[HttpGet, DFMAuthorize]
		public ActionResult RemoveTFA()
		{
			var model = new UsersRemoveTFAModel();
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, DFMAuthorize]
		public ActionResult RemoveTFA(UsersRemoveTFAModel model)
		{
			model.Remove(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}
	}
}