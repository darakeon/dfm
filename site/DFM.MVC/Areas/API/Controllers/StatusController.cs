using System.Web.Mvc;

namespace DFM.MVC.Areas.API.Controllers
{
    public class StatusController : BaseJsonController
	{
        public ActionResult Index()
        {
            return JsonGet(() => new { status = "online" });
        }
    }
}