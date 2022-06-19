using System;
using DFM.BusinessLogic.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[NoWizard]
	public class TokensController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			var model = new TokensIndexModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Index(TokensIndexModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.Test();
				addErrors(errors);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			return RedirectToAction(
				model.SecurityAction.ToString(),
				new { id = model.Token }
			);
		}

		[HttpGetAndHead]
		public IActionResult Mail(PathType path, String token)
		{
			return RedirectToAction(getAction(path), new {id = token});
		}

		private String getAction(PathType pathType)
		{
			return pathType switch
			{
				PathType.PasswordReset => "PasswordReset",
				PathType.UserVerification => "UserVerification",
				PathType.UnsubscribeMoveMail => "UnsubscribeMoveMail",
				PathType.DisableToken => "Disable",
				_ => throw new NotImplementedException()
			};
		}

		[HttpGetAndHead]
		public IActionResult PasswordReset(String id)
		{
			var model = new TokensPasswordResetModel();

			var isValid = model.TestToken(id);

			return isValid
				? View(model)
				: baseModelView("Invalid");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult PasswordReset(String id, TokensPasswordResetModel model)
		{
			var isValid = model.TestToken(id);

			if (!isValid)
				return baseModelView("Invalid");

			if (ModelState.IsValid)
			{
				var errors = model.PasswordReset(id);

				addErrors(errors);
			}

			return ModelState.IsValid
				? baseModelView("PasswordResetSuccess")
				: View(model);
		}

		[HttpGetAndHead]
		public IActionResult UserVerification(String id)
		{
			var model = new SafeModel();

			var isValid = model.TestAndActivate(id);

			return isValid
				? baseModelView("UserVerificationSuccess")
				: baseModelView("Invalid");
		}

		[HttpGetAndHead]
		public IActionResult UnsubscribeMoveMail(String id)
		{
			var model = new AdminModel();

			var isValid = model.TestAndUnsubscribe(id);

			return isValid
				? baseModelView("UnsubscribeMoveMailSuccess")
				: baseModelView("Invalid");
		}

		[HttpGetAndHead]
		public IActionResult Disable(String id)
		{
			var model = new SafeModel();

			var isValid = model.Disable(id);

			return isValid
				? baseModelView()
				: baseModelView("Invalid");
		}
	}
}
