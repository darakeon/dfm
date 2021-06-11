using System;
using System.Collections.Generic;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class ConfigsController : BaseController
	{
		[HttpGetAndHead, Auth]
		public IActionResult Config()
		{
			return View(new ConfigsConfigModel());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigMain(ConfigsConfigModel model)
		{
			return config(model, () => model.Main.Save());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigPassword(ConfigsConfigModel model)
		{
			return config(model, () => model.Info.ChangePassword());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigEmail(ConfigsConfigModel model)
		{
			return config(model, () => model.Info.UpdateEmail());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigTheme(ConfigsConfigModel model)
		{
			return config(model, () => model.ThemeOpt.Change());
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult ConfigTFA(ConfigsConfigModel model)
		{
			return config(model, () => model.TFA.Change());
		}

		private IActionResult config(ConfigsConfigModel model, Func<IList<String>> save)
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
		public IActionResult TFAPasswordEnable()
		{
			var model = new ConfigsTFAModel();
			model.UseAsPassword(true);
			return RedirectToAction("Config");
		}

		[HttpGetAndHead]
		public IActionResult TFAPasswordDisable()
		{
			var model = new ConfigsTFAModel();
			model.UseAsPassword(false);
			return RedirectToAction("Config");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult ChangeLanguageOffline(ConfigsConfigModel model)
		{
			HttpContext.Session.SetString("Language", model.Main.Language);
			return Redirect(model.BackTo);
		}

		[HttpPost, ValidateAntiForgeryToken, Auth]
		public IActionResult EndWizard()
		{
			var model = new ConfigsEndWizardModel();
			return View(model);
		}
	}
}