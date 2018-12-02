using System;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
	[DFMAuthorize]
	public class MovesController : BaseAccountsController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[HttpGet]
		public ActionResult Create()
		{
			var model = new MovesCreateEditModel(OperationType.Creation);
			model.SetDefaultAccount();

			return View("CreateEditSchedule", model);
		}

		[HttpPost]
		public ActionResult Create(MovesCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEditSchedule(model);
		}

		[HttpGet]
		public ActionResult Edit(Int32? id)
		{
			if (!id.HasValue)
				return RedirectToAction("Create");

			var model = new MovesCreateEditModel(id.Value, OperationType.Edition);

			return View("CreateEditSchedule", model);
		}

		[HttpPost]
		public ActionResult Edit(Int32 id, MovesCreateEditModel model)
		{
			model.Move.ID = id;

			return createEditSchedule(model);
		}

		[HttpShouldBePost]
		public ActionResult Delete(Int32 id)
		{
			var model = new MoneyModel();

			model.DeleteMove(id);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}

		[HttpShouldBePost]
		public ActionResult Check(Int32 id)
		{
			var model = new MoneyModel();

			model.CheckMove(id);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}

		[HttpShouldBePost]
		public ActionResult Uncheck(Int32 id)
		{
			var model = new MoneyModel();

			model.UncheckMove(id);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}
	}
}
