using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;

namespace DFM.MVC.Areas.API.Controllers
{
    public class MovesController : BaseJsonController
    {
        public ActionResult List(String accounturl, Int32? id)
        {
            var model = new MovesListModel(accounturl, id);

            return JsonGet(model);
        }

    }
}
