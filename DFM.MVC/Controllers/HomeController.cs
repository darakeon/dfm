using System.Web.Mvc;
using DFM.MVC.Authentication;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Current.IsAuthenticated
                ? RedirectToAction("Index", "Account") 
                : RedirectToAction("LogOn", "User");
        }

        public ActionResult AnalyzeDictionary()
        {
            if (Current.IsAuthenticated)
                return View(PlainText.Dictionary);

            return View(new PlainText());
        }
    }
}
