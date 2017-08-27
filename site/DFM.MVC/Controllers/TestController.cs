using System.Web.Mvc;
using DFM.Multilanguage;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Controllers
{
    [DFMAuthorize(true)]
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View("AnalyzeDictionary", PlainText.Dictionary);
        }

    }
}
