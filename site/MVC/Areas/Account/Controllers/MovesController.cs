using System;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Auth]
	public class MovesController : BaseAccountsController
	{
		[HttpGetAndHead]
		public ActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[HttpGetAndHead]
		public ActionResult Create()
		{
			var model = new MovesCreateEditModel(OperationType.Creation);
			model.SetDefaultAccount();

			return View("CreateEditSchedule", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Create(MovesCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEditSchedule(model);
		}

		[HttpGetAndHead]
		public ActionResult Edit(Int32? id)
		{
			if (!id.HasValue)
				return RedirectToAction("Create");

			var model = new MovesCreateEditModel(id.Value, OperationType.Edition);

			return View("CreateEditSchedule", model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Edit(Int32 id, MovesCreateEditModel model)
		{
			model.Move.ID = id;

			return createEditSchedule(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Delete(Int32 id)
		{
			var model = new MoneyModel();

			model.DeleteMove(id);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Check(Int32 id, PrimalMoveNature nature)
		{
			var model = new MoneyModel();

			model.CheckMove(id, nature);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult Uncheck(Int32 id, PrimalMoveNature nature)
		{
			var model = new MoneyModel();

			model.UncheckMove(id, nature);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}
	}
}
