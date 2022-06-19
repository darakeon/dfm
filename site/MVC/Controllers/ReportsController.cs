using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class ReportsController : BaseController
	{
		[HttpPost, ValidateAntiForgeryToken]
		public IActionResult Search(ReportsSearchModel model)
		{
			model.Search();
			return View(model);
		}

		[HttpPost, NoWizard]
		public IActionResult DismissTip()
		{
			ReportsModel.DismissTip();
			return new JsonResult(new {});
		}
	}
}
