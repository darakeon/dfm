using System;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Account.Controllers
{
	[Auth, Area(Route.AccountArea)]
	public class ReportsController : BaseAccountsController
	{
		[HttpGetAndHead, NoWizard]
		public IActionResult Index()
		{
			return RedirectToAction("Month");
		}

		[HttpGetAndHead]
		public IActionResult Month(Int32? id)
		{
			var model = new ReportsMonthModel(id);

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Year(Int16? id)
		{
			var model = new ReportsYearModel(id);

			return View(model);
		}

		[HttpGetAndHead]
		public IActionResult Categories(Int32? id)
		{
			var model = new ReportsCategoriesModel(id);

			return View(model);
		}
	}
}
