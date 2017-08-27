using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
    [DFMAuthorize]
    public class DetailsController : Controller
    {
        public ActionResult Add(Int32 position = 0)
        {
            var model = new DetailsAddModel(position);

            return View(model);
        }


    }
}
