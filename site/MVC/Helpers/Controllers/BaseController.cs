using System;
using System.Collections.Generic;
using DFM.BusinessLogic;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Helpers.Controllers
{
	public abstract class BaseController : Controller
	{
		protected Current current => HttpContext.GetService().Current;

		protected void addErrors(IList<String> errors)
		{
			if (errors == null)
				return;

			foreach (var error in errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		[AspMvcView]
		protected IActionResult baseModelView()
		{
			return View(new BaseSiteModel());
		}

		protected IActionResult baseModelView([AspMvcView] String view)
		{
			return View(view, new BaseSiteModel());
		}
	}
}
