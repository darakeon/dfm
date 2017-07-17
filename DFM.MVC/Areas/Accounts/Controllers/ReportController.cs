using System;
using System.Globalization;
using System.Web.Mvc;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        readonly AccountData accountData = new AccountData();



        public ActionResult SeeMonth(Int32? id, Int32 accountid)
        {
            var month = id.HasValue
                ? id.Value % 100
                : DateTime.Now.Month;

            var year = id.HasValue
                ? id.Value / 100
                : DateTime.Now.Year;

            
            var model = new ReportSeeMonthModel
                            {
                                Month = DateTimeFormatInfo.InvariantInfo.GetMonthName(month),
                                Year = year, 
                                MoveList = accountData.GetMonthReport(accountid, month, year)
                            };


            return View(model);
        }



        public ActionResult SeeYear(Int32? id, Int32 accountid)
        {
            var year = id ?? DateTime.Now.Year;

            var model = new ReportSeeYearModel
                            {
                                Year = year,
                                MoveSumList = accountData.GetYearReport(accountid, year)
                            };


            return View(model);
        }
    }
}
