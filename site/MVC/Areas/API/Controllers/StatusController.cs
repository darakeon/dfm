using System.Web.Mvc;

namespace DFM.MVC.Areas.API.Controllers
{
	public class StatusController : BaseJsonController
	{
		[HttpGet]
		public ActionResult Index()
		{
			return json(() => new { status = "online" });
		}
	}
}