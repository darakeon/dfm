using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
	[DFMAuthorize]
	public class SchedulesController : BaseAccountsController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[HttpGet]
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
