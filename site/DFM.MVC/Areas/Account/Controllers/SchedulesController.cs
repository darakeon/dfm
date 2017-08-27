using System.Web.Mvc;
using DFM.Generic;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
    [DFMAuthorize]
    public class SchedulesController : BaseAccountsController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }



        public ActionResult Create()
        {
            var model = new ScheduleCreateModel();

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(ScheduleCreateModel model)
        {
            model.Type = OperationType.Schedule;

            return CreateEditSchedule(model);
        }



    }
}
