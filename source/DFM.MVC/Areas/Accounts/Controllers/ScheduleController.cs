using System.Web.Mvc;
using DFM.Generic;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [DFMAuthorize]
    public class ScheduleController : BaseAccountsController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }



        public ActionResult Create()
        {
            var model = new ScheduleCreateModel();

            model.PopulateExcludingAccount(model.Account.Name);

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
