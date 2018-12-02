using System.Web.Mvc;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Account.Controllers
{
	public class BaseAccountsController : BaseController
	{
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
