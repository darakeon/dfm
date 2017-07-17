using System.Web.Mvc;
using DFM.MVC.Authentication;

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
    }
}
