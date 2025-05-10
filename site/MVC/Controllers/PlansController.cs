using DFM.MVC.Authorize;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers;

[Auth]
public class PlansController : BaseController
{
	[HttpGetAndHead]
	public IActionResult Index()
	{
		return View(new PlansIndexModel());
	}
}
