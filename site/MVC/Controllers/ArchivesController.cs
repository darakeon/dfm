using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers;

public class ArchivesController : BaseController
{
	[HttpGetAndHead]
	public IActionResult Index()
	{
		var model = new ArchivesIndexModel();

		return View(model);
	}


}