using System;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Area(Route.AccountArea), Auth]
	public class ReportsController : BaseAccountsController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return RedirectToAction("ShowMoves");
		}

		[HttpGetAndHead]
		public IActionResult ShowMoves(Int32? id)
		{
			var model = new ReportsShowMovesModel(id);

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult SummarizeMonths(Int16? id)
		{
			var model = new ReportsSummarizeMonthsModel(id);

			return View(model);
		}
	}
}
