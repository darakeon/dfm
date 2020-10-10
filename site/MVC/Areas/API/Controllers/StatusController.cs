using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea)]
	public class StatusController : BaseJsonController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return json(() => new { status = "online" });
		}
	}
}
