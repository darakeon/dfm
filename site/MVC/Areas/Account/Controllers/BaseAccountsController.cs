using DFM.Entities.Bases;
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
				var yearMonth = model.GenericMove
					.GetDate().ToString("yyyyMM");

				return RedirectToAction(
					"Month", "Reports",
					new { id = yearMonth }
				);
			}

			return View("CreateEditSchedule", model);
		}
	}
}
