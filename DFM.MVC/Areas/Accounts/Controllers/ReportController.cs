using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Entities;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private Account account;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var url = RouteData.Values["accounturl"].ToString();
            account = Current.User.AccountList.SingleOrDefault(a => a.Url == url);
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
                                MoveList = Services.Report.GetMonthReport(account.Name, dateMonth, dateYear),
                                Account = account,
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
                                Year =  Services.Report.GetYearReport(account.Name, year),
                            };


            return View(model);
        }




    }
}
