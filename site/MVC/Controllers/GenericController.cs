using System;
using System.Text;
using DFM.Generic;
using DFM.MVC.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class GenericController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Mobile()
		{
			return Redirect(Cfg.GooglePlay);
		}

		public IActionResult Reload()
		{
			return Content(" ");
		}

	}
}
