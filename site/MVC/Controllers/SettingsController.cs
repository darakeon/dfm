﻿using System;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class SettingsController : BaseController
	{
		[Auth, HttpGetAndHead]
		public IActionResult Index()
		{
			return View(new SettingsIndexModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Index(SettingsIndexModel model)
		{
			return settings(model, "Index");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Password()
		{
			return View(new SettingsPasswordModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Password(SettingsPasswordModel model)
		{
			return settings(model, "Password");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Email()
		{
			return View(new SettingsEmailModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Email(SettingsEmailModel model)
		{
			return settings(model, "Email");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Theme()
		{
			return View(new SettingsThemeModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Theme(SettingsThemeModel model)
		{
			return settings(model, "Theme");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Wipe()
		{
			return View(new SettingsWipeModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Wipe(SettingsWipeModel model)
		{
			return settings(model, "Wipe");
		}

		[Auth, HttpGetAndHead]
		public IActionResult TFA()
		{
			return View(new SettingsTFAModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult TFA(SettingsTFAModel model)
		{
			return settings(model, "TFA");
		}

		[Auth, HttpGetAndHead]
		public IActionResult TFAPassword()
		{
			return View(new SettingsTFAPasswordModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult TFAPassword(SettingsTFAPasswordModel model)
		{
			return settings(model, "TFAPassword");
		}

		private IActionResult settings(SettingsModel model, [AspMvcView] String view)
		{
			if (ModelState.IsValid)
				addErrors(model.Save());

			if (!ModelState.IsValid)
				return View(view, model);

			var saferBackTo = fixCWE601(model.BackTo);
			if (!String.IsNullOrEmpty(saferBackTo))
				return Redirect(saferBackTo);

			return RedirectToAction();
		}

		[HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
		public IActionResult ChangeLanguageOffline(SettingsIndexModel model)
		{
			HttpContext.Session.SetString(
				"Language", model.NewLanguage
			);
			var saferBackTo = fixCWE601(model.BackTo);
			return Redirect(saferBackTo ?? "/");
		}

		[Auth, HttpPost, ValidateAntiForgeryToken, Wizard.Avoid]
		public IActionResult EndWizard()
		{
			var model = new SettingsEndWizardModel();
			return View(model);
		}

		[Auth, HttpGetAndHead]
		public IActionResult Misc()
		{
			return View(new SettingsMiscModel());
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Misc(SettingsMiscModel model)
		{
			return settings(model, "Misc");
		}
	}
}
