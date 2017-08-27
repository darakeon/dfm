using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.Account.Controllers
{
    [DFMAuthorize]
    public class ReportsController : BaseAccountsController
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
