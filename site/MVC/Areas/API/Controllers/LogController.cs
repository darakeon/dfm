using System;
using DfM.Logs;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea), Auth(AuthParams.Admin)]
	public class LogController : BaseJsonController
	{
		[HttpGetAndHead]
		public IActionResult Count()
		{
			return json(() => new LogFile(false));
		}

		[HttpGetAndHead]
		public IActionResult List()
		{
			return json(() => new LogFile(true));
		}

		[HttpPost, HttpGetAndHead]
		public IActionResult Archive(Int32 id)
		{
			return json(() => new LogFile(true).Archive(id));
		}
	}
}
