using System;
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

		[Auth, HttpGetAndHead]
		public IActionResult Email()
		{
			return View(new ConfigsEmailModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Email(ConfigsEmailModel model)
		{
			return config(model, "Email");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Theme()
		{
			return View(new ConfigsThemeModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Theme(ConfigsThemeModel model)
		{
			return config(model, "Theme");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Wipe()
		{
			return View(new ConfigsWipeModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Wipe(ConfigsWipeModel model)
		{
			return config(model, "Wipe");
		}

		[Auth, HttpGetAndHead]
		public IActionResult TFA()
		{
			return View(new ConfigsTFAModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult TFA(ConfigsTFAModel model)
		{
			return config(model, "TFA");
		}

		private IActionResult config(ConfigsModel model, [AspMvcView] String view)
		{
			if (ModelState.IsValid)
				addErrors(model.Save());

			if (!ModelState.IsValid)
				return View(view, model);

			if (!String.IsNullOrEmpty(model.BackTo))
				return Redirect(model.BackTo);

			return RedirectToAction();
		}

		[Auth, HttpGetAndHead, Wizard.Avoid]
		public IActionResult TFAPasswordEnable()
		{
			var model = new ConfigsTFAPasswordModel();
			model.UseAsPassword(true);
			return RedirectToAction("TFA");
		}

		[Auth, HttpGetAndHead, Wizard.Avoid]
		public IActionResult TFAPasswordDisable()
		{
			var model = new ConfigsTFAPasswordModel();
			model.UseAsPassword(false);
			return RedirectToAction("TFA");
		}

		[HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
		public IActionResult ChangeLanguageOffline(ConfigsIndexModel model)
		{
			HttpContext.Session.SetString("Language", model.NewLanguage);
			return Redirect(model.BackTo);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
		public IActionResult EndWizard()
		{
			var model = new ConfigsEndWizardModel();
			return View(model);
		}

		[Auth, HttpGetAndHead, Wizard.Avoid]
		public IActionResult Misc()
		{
			return View(new ConfigsMiscModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
		public IActionResult Misc(ConfigsMiscModel model)
		{
			return config(model, "Misc");
		}
	}
}
