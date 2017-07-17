using System;
using System.Globalization;
using System.Web.Mvc;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.MVC.Helpers;
using DFM.MVC.MultiLanguage;

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
                                MoveList = accountData.GetMonthReport(accountid, month, year),
                                Account = accountData.SelectById(accountid),

                                Date = 
                                    String.Format(PlainText.Dictionary["ShortDateFormat"],
                                        PlainText.GetMonthName(month), year)
                            };


            return View(model);
        }



        public ActionResult SeeYear(Int32? id, Int32 accountid)
        {
            var year = id ?? DateTime.Now.Year;

            var model = new ReportSeeYearModel
                            {
                                Year = accountData.GetYearReport(accountid, year),
                                Account = accountData.SelectById(accountid),
                            };


            return View(model);
        }
    }
}
