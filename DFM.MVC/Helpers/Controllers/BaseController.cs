using System.Web.Mvc;
using DFM.Authentication;

namespace DFM.MVC.Helpers.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Current Current = Auth.Current; 

    }

}