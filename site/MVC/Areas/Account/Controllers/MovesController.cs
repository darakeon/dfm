using System;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Area(Route.AccountArea)]
	public class MovesController : BaseAccountsController
	{
		[Auth, HttpGetAndHead]
		public IActionResult Index()
		{
			return RedirectToAction("Create");
		}

		[Auth, HttpGetAndHead]
		public IActionResult Create()
		{
			var model = new MovesCreateEditModel();
			model.SetDefaultAccount();

			return View("CreateEditSchedule", model);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Create(MovesCreateEditModel model)
		{
			model.Type = OperationType.Creation;

			return createEditSchedule(model);
		}

		[Auth, HttpGetAndHead]
		public IActionResult Edit(Int32? id)
		{
			if (!id.HasValue)
				return RedirectToAction("Create");

			var model = new MovesCreateEditModel(id.Value);

			return View("CreateEditSchedule", model);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Edit(Int32 id, MovesCreateEditModel model)
		{
			model.ID = id;

			return createEditSchedule(model);
		}

		[Auth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Delete(Int32 id)
		{
			var model = new MoneyModel();

			model.DeleteMove(id);

			return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
		}

		[JsonAuth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Check(Int32 id, PrimalMoveNature nature)
		{
			var model = new MoneyModel();

			var line = model.CheckMove(id, nature);

			return PartialView("../Reports/MoveLine", line);
		}

		[JsonAuth, HttpPost, ValidateAntiForgeryToken]
		public IActionResult Uncheck(Int32 id, PrimalMoveNature nature)
		{
			var model = new MoneyModel();

			var line = model.UncheckMove(id, nature);

			return PartialView("../Reports/MoveLine", line);
		}
	}
}
