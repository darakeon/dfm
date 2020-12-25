using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class UsersController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			var model = new UsersIndexModel();

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult SignUp()
		{
			var model = new UsersSignUpModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult SignUp(UsersSignUpModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.ValidateAndSendVerify();

				addErrors(errors);
			}

			return ModelState.IsValid
				? baseModelView("SignUpSuccess")
				: View(model);
		}

		[HttpGetAndHead]
		public IActionResult LogOn()
		{
			var model = new UsersLogOnModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult LogOn(UsersLogOnModel model, String returnUrl)
		{
			var logOnError = model.TryLogOn();

			if (logOnError == null)
			{
				return String.IsNullOrEmpty(returnUrl)
					? (IActionResult) RedirectToAction("Index", "Accounts")
					: Redirect(returnUrl);
			}

			if (logOnError.Type == Error.DisabledUser)
			{
				return baseModelView("SendVerification");
			}

			ModelState.AddModelError("", HttpContext.Translate(logOnError));

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult LogOff()
		{
			var model = new SafeModel();

			model.LogOff();

			return RedirectToAction("Index", "Users");
		}

		[HttpGetAndHead]
		public IActionResult ForgotPassword()
		{
			var model = new UsersForgotPasswordModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult ForgotPassword(UsersForgotPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.SendPasswordReset();

				addErrors(errors);
			}

			return ModelState.IsValid
				? baseModelView("ForgotPasswordSuccess")
				: View(model);
		}

		[HttpGetAndHead, Auth]
		public IActionResult Config()
		{
			return View(new UsersConfigModel());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigOptions(UsersConfigModel model)
		{
			return config(model, () => model.Main.Save());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigPassword(UsersConfigModel model)
		{
			return config(model, () => model.Info.ChangePassword());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigEmail(UsersConfigModel model)
		{
			return config(model, () => model.Info.UpdateEmail());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigTheme(UsersConfigModel model)
		{
			return config(model, () => model.ThemeOpt.Change());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigTFA(UsersConfigModel model)
		{
			return config(model, () => model.TFA.Change());
		}

		private IActionResult config(UsersConfigModel model, Func<IList<String>> save)
		{
			if (ModelState.IsValid)
				addErrors(save());

			if (!ModelState.IsValid)
				return View("Config", model);

			if (!String.IsNullOrEmpty(model.BackTo))
				return Redirect(model.BackTo);

			return RedirectToAction("Index", "Accounts");
		}

		[HttpGetAndHead]
		public IActionResult Contract()
		{
			var model = new UsersContractModel();
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, Auth(needContract: false)]
		public IActionResult Contract(UsersContractModel model)
		{
			model.AcceptContract(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult EndWizard()
		{
			var model = new UsersEndWizardModel();
			return View(model);
		}

		[HttpGetAndHead, Auth(needContract: false, needTFA: false)]
		public IActionResult TFA()
		{
			var model = new UsersTFAModel();
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken, Auth(needContract: false, needTFA: false)]
		public IActionResult TFA(UsersTFAModel model)
		{
			model.Validate(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult ChangeLanguageOffline(UsersConfigModel model)
		{
			HttpContext.Session.SetString("Language", model.Main.Language);
			return Redirect(model.BackTo);
		}
	}
}
