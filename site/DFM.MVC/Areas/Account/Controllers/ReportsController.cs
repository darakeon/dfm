using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Auth]
	public class ReportsController : BaseAccountsController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("ShowMoves");
		}

		[HttpGet]
		public ActionResult ShowMoves(Int32? id)
		{
			var model = new ReportsShowMovesModel(id);

			return View(model);
		}

		[HttpGet]
		public ActionResult SummarizeMonths(Int16? id)
		{
			var model = new ReportsSummarizeMonthsModel(id);

			return View(model);
		}
	}
}
