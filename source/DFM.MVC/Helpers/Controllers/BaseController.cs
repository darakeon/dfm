using System.Web.Mvc;
using DFM.Authentication;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Helpers.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Current Current = Auth.Current; 

    }

}