using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        readonly AccountData accountData = new AccountData();

        private Int32 accountid;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            accountid = Int32.Parse(RouteData.Values["accountid"].ToString());
        }



        public ActionResult SeeMonth(Int32? id)
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



        public ActionResult SeeYear(Int32? id)
        {
            var year = id ?? DateTime.Now.Year;

            var model = new ReportSeeYearModel
                            {
                                Year = accountData.GetYearReport(accountid, year),
                            };


            return View(model);
        }
    }
}
