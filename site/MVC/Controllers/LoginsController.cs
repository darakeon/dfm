using System;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class LoginsController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return View(new LoginsIndexModel());
		}

		[HttpPost, ValidateAntiForgeryToken, NoWizard]
		public IActionResult Delete(String id)
		{
			var model = new SafeModel();

			model.DisableLogin(id);

			return RedirectToAction("Index");
		}
	}
}
