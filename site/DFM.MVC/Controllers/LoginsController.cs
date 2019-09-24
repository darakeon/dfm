using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class LoginsController : BaseController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View(new LoginsIndexModel());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(String id)
		{
			var model = new SafeModel();

			model.DisableLogin(id);

			return RedirectToAction("Index");
		}
	}
}
