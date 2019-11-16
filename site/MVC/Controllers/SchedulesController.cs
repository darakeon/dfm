using System;
using System.Web.Mvc;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class SchedulesController : BaseController
	{
		[HttpGetAndHead]
		public ActionResult Index()
		{
			return View(new SchedulesIndexModel());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(Int32 id)
		{
			var model = new RobotModel();

			model.Disable(id);

			return RedirectToAction("Index");
		}
	}
}
