using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	public class ReportsController : BaseController
	{
		public IActionResult Search(ReportsSearchModel model)
		{
			model.Search();
			return View(model);
		}

		public IActionResult DismissTip()
		{
			ReportsModel.DismissTip();
			return new JsonResult(new {});
		}
	}
}
