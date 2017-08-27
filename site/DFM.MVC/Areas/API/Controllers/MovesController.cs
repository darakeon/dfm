using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;

namespace DFM.MVC.Areas.API.Controllers
{
    public class MovesController : BaseJsonController
    {
        public ActionResult List(String accounturl)
        {
            var model = new MovesListModel("new", 10, 2013);

            return JsonGet(model);
        }

    }
}
