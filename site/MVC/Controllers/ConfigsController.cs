using System;
using System.Collections.Generic;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class ConfigsController : BaseController
	{
		[Auth, HttpGetAndHead]
		public IActionResult Index()
		{
			return View(new ConfigsIndexModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Index(ConfigsIndexModel model)
		{
			return config(model, "Index");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Password()
		{
			return View(new ConfigsPasswordModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Password(ConfigsPasswordModel model)
		{
			return config(model, "Password");
		}

		[HttpGetAndHead, Auth]
		public IActionResult Config()
		{
			return View(new ConfigsConfigModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult ConfigEmail(ConfigsConfigModel model)
		{
			return config(model, () => model.Info.UpdateEmail());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult ConfigTheme(ConfigsConfigModel model)
		{
			return config(model, () => model.ThemeOpt.Change());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult ConfigTFA(ConfigsConfigModel model)
		{
			return config(model, () => model.TFA.Change());
		}

		private IActionResult config(ConfigsModel model, [AspMvcView] String view)
		{
			if (ModelState.IsValid)
				addErrors(model.Save());

			if (!ModelState.IsValid)
				return View(view, model);

			if (!String.IsNullOrEmpty(model.BackTo))
				return Redirect(model.BackTo);

			return RedirectToAction("Index", "Accounts");
		}

		private IActionResult config(ConfigsConfigModel model, Func<IList<String>> save)
		{
			if (ModelState.IsValid)
				addErrors(save());

			if (!ModelState.IsValid)
				return View(model);

			if (!String.IsNullOrEmpty(model.BackTo))
				return Redirect(model.BackTo);

			return RedirectToAction("Index", "Accounts");
		}

		[Auth, HttpGetAndHead]
		public IActionResult TFAPasswordEnable()
		{
			var model = new ConfigsTFAModel();
			model.UseAsPassword(true);
			return RedirectToAction("Config");
		}

		[Auth, HttpGetAndHead]
		public IActionResult TFAPasswordDisable()
		{
			var model = new ConfigsTFAModel();
			model.UseAsPassword(false);
			return RedirectToAction("Config");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult ChangeLanguageOffline(ConfigsIndexModel model)
		{
			HttpContext.Session.SetString("Language", model.NewLanguage);
			return Redirect(model.BackTo);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult EndWizard()
		{
			var model = new ConfigsEndWizardModel();
			return View(model);
		}
	}
}