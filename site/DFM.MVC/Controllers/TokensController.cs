using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	public class TokensController : BaseController
	{
		[HttpGet]
		public ActionResult Index()
		{
			var model = new TokensIndexModel();

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Index(TokensIndexModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.Test();
				AddErrors(errors);
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

		[HttpGet]
		public ActionResult PasswordReset(String id)
		{
			var model = new TokensPasswordResetModel();

			var isValid = model.TestToken(id);

			return isValid
				? View(model)
				: BaseModelView("Invalid");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult PasswordReset(String id, TokensPasswordResetModel model)
		{
			var isValid = model.TestToken(id);

			if (!isValid)
				return BaseModelView("Invalid");

			if (ModelState.IsValid)
			{
				var errors = model.PasswordReset(id);

				AddErrors(errors);
			}

			return ModelState.IsValid
				? BaseModelView("PasswordResetSuccess")
				: View(model);
		}

		[HttpGet]
		public ActionResult UserVerification(String id)
		{
			var model = new SafeModel();

			var isValid = model.TestAndActivate(id);

			return isValid
				? BaseModelView("UserVerificationSuccess")
				: BaseModelView("Invalid");
		}

		[HttpGet]
		public ActionResult Disable(String id)
		{
			var model = new SafeModel();

			var isValid = model.Disable(id);

			return isValid
				? BaseModelView()
				: BaseModelView("Invalid");
		}
	}
}
