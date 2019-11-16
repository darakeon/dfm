using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Auth]
	public class SchedulesController : BaseAccountsController
	{
		[HttpGetAndHead]
		public ActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[HttpGetAndHead]
		public ActionResult Create()
		{
			var model = new SchedulesCreateModel();
			model.SetDefaultAccount();

			return View("CreateEditSchedule", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(SchedulesCreateModel model)
		{
			model.Type = OperationType.Scheduling;

			return createEditSchedule(model);
		}
	}
}
