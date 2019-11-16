using System;
using System.Web.Mvc;
using Keon.Util.Collection;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using elmah = DFM.Generic.Elmah;

namespace DFM.MVC.Controllers
{
	public class OpsController : Controller
	{
		[HttpGetAndHead]
		public ActionResult Index()
		{
			var model = new OpsModel();

			return View(model);
		}

		[HttpGetAndHead]
		public ActionResult Code(Int32 id)
		{
			if (!id.IsIn(404, 500))
				return RedirectToAction("Index");

			var model = new OpsCodeModel
			{
				EmailSent = ErrorManager.EmailSent
			};

			return View(id.ToString(), model);
		}

		[HttpGetAndHead]
		public ActionResult Error(Error id)
		{
			var model = new OpsModel(id);

			return View(model);
		}

		[HttpGetAndHead]
		public ActionResult Help()
		{
			var model = new BaseSiteModel();

			return View(model);
		}

		[HttpGetAndHead]
		public ActionResult TestElmahLog()
		{
			var errorOnLog = elmah.TryLog(new Exception("Logging right!"));
			return Content(errorOnLog?.ToString());
		}

		[HttpGetAndHead]
		public ActionResult Legend()
		{
			var model = new BaseSiteModel();

			return View(model);
		}
	}
}
