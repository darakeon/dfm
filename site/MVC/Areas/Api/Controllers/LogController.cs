using System;
using DFM.MVC.Areas.Api.Models;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea)]
	public class LogController : BaseJsonController
	{
		[HttpGetAndHead]
		public IActionResult Count()
		{
			return json(() => new LogModel(false));
		}

		[HttpGetAndHead]
		public IActionResult List()
		{
			return json(() => new LogModel(true));
		}

		[HttpPost]
		public IActionResult Archive(String id)
		{
			return json(() => new LogModel(false).Archive(id));
		}
	}
}
