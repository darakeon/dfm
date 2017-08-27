using System.Web.Mvc;
using DFM.Multilanguage;

namespace DFM.MVC.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View("AnalyzeDictionary", PlainText.Dictionary);
        }

    }
}
