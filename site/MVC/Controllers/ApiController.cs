using System;
using System.Collections.Generic;
using DFM.BaseWeb.Helpers.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;
using DFM.MVC.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Wizard.Avoid]
	public class ApiController : Controller
	{
		public IActionResult Index()
		{
			return Json(new Dictionary<String, Object>
			{
				{ "code", Error.UpdateApp },
				{ "error", HttpContext.Translate("UpdateApp") + Cfg.Version },
			});
		}
	}
}
