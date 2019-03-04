using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Controllers
{
	public class BaseAccountsController : BaseController
	{
		protected override void OnException(ExceptionContext filterContext)
		{
			if (
				filterContext.Exception is DFMCoreException exception
				&&
				exception.Type == ExceptionPossibilities.InvalidAccount
			)
			{
				ErrorAlert.Add(exception.Type);

				filterContext.Result = 
					RedirectToRoute(
						RouteNames.DEFAULT, 
						new { controller = "Accounts" }
					);

				filterContext.ExceptionHandled = true;
			}
			else
			{
				base.OnException(filterContext);
			}
		}

		protected ActionResult createEditSchedule(BaseMovesModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.CreateEditSchedule();
				AddErrors(errors);
			}

			if (ModelState.IsValid)
			{
				return RedirectToAction(
					"ShowMoves", "Reports",
					new { id = model.Date.ToString("yyyyMM") }
				);
			}

			return View("CreateEditSchedule", model);
		}
	}
}
