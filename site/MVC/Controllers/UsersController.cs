using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Wizard.Avoid]
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
				var errors = model.SaveUser();
				addErrors(errors);
			}

			return ModelState.IsValid
				? RedirectToAction("Index", "Accounts")
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
				var saferReturnUrl = fixCWE601(returnUrl);
				return String.IsNullOrEmpty(saferReturnUrl)
					? RedirectToAction("Index", "Accounts")
					: Redirect(saferReturnUrl);
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

		[HttpGetAndHead]
		public IActionResult Contract()
		{
			var model = new UsersContractModel();
			return View(model);
		}

		[Auth(AuthParams.IgnoreContract), HttpPost, ValidateAntiForgeryToken]
		public IActionResult Contract(UsersContractModel model)
		{
			model.AcceptContract(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[Auth(AuthParams.IgnoreContract | AuthParams.IgnoreTFA), HttpGetAndHead]
		public IActionResult TFA()
		{
			var model = new UsersTFAModel();
			return View(model);
		}

		[Auth(AuthParams.IgnoreContract | AuthParams.IgnoreTFA), HttpPost, ValidateAntiForgeryToken]
		public IActionResult TFA(UsersTFAModel model)
		{
			model.Validate(ModelState.AddModelError);

			if (ModelState.IsValid)
			{
				return RedirectToAction("Index", "Accounts");
			}

			return View(model);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Verification(UsersVerificationModel model)
		{
			model.Send(ModelState.AddModelError);
			return View(model);
		}
	}
}
