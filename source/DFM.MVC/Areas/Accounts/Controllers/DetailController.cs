using System;
using System.Web.Mvc;
using DFM.Entities;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class DetailController : Controller
    {
        public ActionResult Add(Int32 position = 0, Int32 id = 0)
        {
            var detail = id == 0
                ? default(Detail)
                : Services.Money.GetDetailById(id);

            var model = new MoveAddDetailModel(position, detail);

            return View(model);
        }


        


    }
}
