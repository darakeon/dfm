﻿using System;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth]
	public class SchedulesController : BaseController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return View(new SchedulesIndexModel());
		}

		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Delete(Int32 id)
		{
			var model = new RobotModel();

			model.Disable(id);

			return RedirectToAction("Index");
		}
	}
}
