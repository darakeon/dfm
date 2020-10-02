using System;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class ReportsController : Controller
	{
		public IActionResult Search(ReportsSearchModel model)
		{
			model.Search();
			return View(model);
		}
	}
}
