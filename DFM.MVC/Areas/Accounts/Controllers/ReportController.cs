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



        public ActionResult SeeMonth(Int16? id)
        {
            var currentMonth = (Int16)DateTime.Today.Month;

            var month = id.HasValue
                ? id.Value % 100
                : currentMonth;

            month = month.ForceBetween(1, 12);


            var currentYear = DateTime.Today.Year;

            var year = id.HasValue
                ? id.Value / 100
                : currentYear;

            year = year.ForceBetween(1900, currentYear);


            var model = new ReportSeeMonthModel
                            {
                                MoveList = AccountData.GetMonthReport(accountid, month, year),
                                Account = AccountData.SelectById(accountid),
                                Month = month,
                                Year = year,
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
