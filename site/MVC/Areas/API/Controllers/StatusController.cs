using System.Web.Mvc;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Controllers
{
	public class StatusController : BaseJsonController
	{
		[HttpGetAndHead]
		public ActionResult Index()
		{
			return json(() => new { status = "online" });
		}
	}
}
