using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DFM.BusinessLogic;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Controllers
{
	public class BaseController : Controller
	{
		protected readonly Current Current = Service.Current;


		protected void AddErrors(IList<String> errors)
		{
			if (errors == null)
				return;

			foreach (var error in errors)
			{
				ModelState.AddModelError("", error);
			}
		}


		[AspMvcView]
		protected ActionResult BaseModelView()
		{
			return View(new BaseSiteModel());
		}

		protected ActionResult BaseModelView([AspMvcView] String view)
		{
			return View(view, new BaseSiteModel());
		}

		protected override void HandleUnknownAction(String actionName)
		{
			var ignoreCase = StringComparison.CurrentCultureIgnoreCase;

			var wrongHttpMethod = 
				GetType().GetMethods().Any(
					m => m.IsPublic && m.Name.Equals(actionName, ignoreCase)
				);

			if (wrongHttpMethod)
			{
				RedirectToRoute(
					RouteNames.DEFAULT,
					new
					{
						controller = "Ops",
						action = "Code",
						id = "404"
					}
				).ExecuteResult(ControllerContext);
			}
			else
			{
				base.HandleUnknownAction(actionName);
			}
		}
	}
}
