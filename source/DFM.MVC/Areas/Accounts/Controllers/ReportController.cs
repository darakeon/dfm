using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Entities;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [DFMAuthorize]
    public class ReportController : BaseAccountsController
    {
        public ActionResult Index()
        {
            return RedirectToAction("ShowMoves");
        }



        public ActionResult ShowMoves(Int32? id)
        {
            var model = new ReportShowMovesModel(id);

            return View(model);
        }



        public ActionResult SummarizeMonths(Int16? id)
        {
            var model = new ReportSummarizeMonthsModel(id);

            return View(model);
        }




    }
}
