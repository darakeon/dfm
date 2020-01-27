using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Area(Route.AccountArea), Auth]
	public class SchedulesController : BaseAccountsController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[HttpGetAndHead]
		public IActionResult Create()
		{
			var model = new SchedulesCreateModel();
			model.SetDefaultAccount();

			return View("CreateEditSchedule", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Create(SchedulesCreateModel model)
		{
			model.Type = OperationType.Scheduling;

			return createEditSchedule(model);
		}
	}
}
