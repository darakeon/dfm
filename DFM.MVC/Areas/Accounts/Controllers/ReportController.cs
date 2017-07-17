using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.MVC.Helpers.Extensions;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private Int32 accountid;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            accountid = Int32.Parse(RouteData.Values["accountid"].ToString());
        }



        public ActionResult Index()
        {
            return RedirectToAction("SeeMonth");
        }



        public ActionResult SeeMonth(Int32? id)
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


            var model = new ReportSeeMonthModel
                            {
                                MoveList = AccountData.GetMonthReport(accountid, dateMonth, dateYear),
                                Account = AccountData.SelectById(accountid),
                                Month = dateMonth,
                                Year = dateYear,
                            };


            return View(model);
        }



        public ActionResult SeeYear(Int16? id)
        {
            var currentYear = (Int16)DateTime.Today.Year;
            
            var year = id ?? currentYear;
            
            year = year.ForceBetween(1900, currentYear);


            var model = new ReportSeeYearModel
                            {
                                Year = AccountData.GetYearReport(accountid, year),
                            };


            return View(model);
        }
    }
}
