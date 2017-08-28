using System.Web.Mvc;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[DFMAuthorize(true)]
	public class TestController : Controller
	{
		public ActionResult Index()
		{
			return View("AnalyzeDictionary", new TestAnalyzeDictionary());
		}

	}
}
