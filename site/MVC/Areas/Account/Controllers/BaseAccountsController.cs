using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Account.Controllers
{
	public class BaseAccountsController : BaseController
	{
		protected IActionResult createEditSchedule(BaseMovesModel model)
		{
			if (ModelState.IsValid)
			{
				var errors = model.CreateEditSchedule();
				addErrors(errors);
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
