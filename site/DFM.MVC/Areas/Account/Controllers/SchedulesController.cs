using System.Web.Mvc;
using DFM.Generic;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
	[DFMAuthorize]
	public class SchedulesController : BaseAccountsController
	{
		public ActionResult Index()
		{
			return RedirectToAction("Create");
		}



		public ActionResult Create()
		{
			var model = new SchedulesCreateModel();

			return View("Create", model);
		}

		[HttpPost]
		public ActionResult Create(SchedulesCreateModel model)
		{
			model.Type = OperationType.Schedule;

			return CreateEditSchedule(model);
		}



	}
}
