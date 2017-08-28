using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DFM.Authentication;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Helpers.Controllers
{
	public class BaseController : Controller
	{
		protected readonly Current Current = Auth.Current;


		protected void AddErrors(IList<String> errors)
		{
			if (errors == null)
				return;

			foreach (var error in errors)
			{
				ModelState.AddModelError("", error);
			}
		}


	}

}