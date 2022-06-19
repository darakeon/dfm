using System;
using Keon.Util.Collection;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[NoWizard]
	public class OpsController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			var model = new OpsModel();
			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Code(Int32 id)
		{
			if (!id.IsIn(404, 500))
				return RedirectToAction("Index");

			var errorManager = new ErrorManager(HttpContext);

			var model = new OpsCodeModel
			{
				EmailSent = errorManager.EmailSent
			};

			return View(id.ToString(), model);
		}

		[HttpGetAndHead]
		public IActionResult Error(Error id)
		{
			var model = new OpsModel(id);
			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Help()
		{
			var model = new BaseSiteModel();
			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Legend()
		{
			var model = new BaseSiteModel();
			return View(model);
		}
	}
}
