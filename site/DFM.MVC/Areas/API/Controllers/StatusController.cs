using System.Web.Mvc;

namespace DFM.MVC.Areas.API.Controllers
{
    public class StatusController : BaseJsonController
	{
        public ActionResult Index()
        {
            return json(() => new { status = "online" });
        }
    }
}