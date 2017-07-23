using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private String accountname;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            accountname = RouteData.Values["accountname"].ToString();
        }



        public ActionResult Index()
        {
            return RedirectToAction("ShowMoves");
        }



        public ActionResult ShowMoves(Int32? id)
        {
            var currentMonth = (Int16)DateTime.Today.Month;

            var dateMonth = id.HasValue
                ? (Int16)(id.Value % 100)
                : currentMonth;

            dateMonth = dateMonth.ForceBetween(1, 12);


            var currentYear = (Int16)DateTime.Today.Year;

            var dateYear = id.HasValue
                ? (Int16)(id.Value / 100)
                : currentYear;

            dateYear = dateYear.ForceBetween(1900, currentYear);


            var model = new ReportShowMovesModel
                            {
                                MoveList = Services.Report.GetMonthReport(accountname, dateMonth, dateYear),
                                Account = Services.Admin.SelectAccountByName(accountname),
                                Month = dateMonth,
                                Year = dateYear,
                            };


            return View(model);
        }



        public ActionResult SummarizeMonths(Int16? id)
        {
            var currentYear = (Int16)DateTime.Today.Year;
            
            var year = id ?? currentYear;
            
            year = year.ForceBetween(1900, currentYear);


            var model = new ReportSummarizeMonthsModel
                            {
                                Year =  Services.Report.GetYearReport(accountname, year),
                            };


            return View(model);
        }




    }
}
