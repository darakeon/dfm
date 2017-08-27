using System;
using DFM.Entities;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSummarizeMonthsModel : BaseAccountsModel
    {
        public ReportSummarizeMonthsModel(Int16? id)
        {
            var year = DateFromInt.GetDateYear(id, Today);

            Year = Report.GetYearReport(AccountUrl, year);
        }



        public Year Year { get; set; }

        public String Date
        {
            get
            {
                return String.Format(MultiLanguage.Dictionary["ShortDateFormat"],
                                     MultiLanguage.Dictionary["Summary"], Year.Time);
            }
        }


    }
}