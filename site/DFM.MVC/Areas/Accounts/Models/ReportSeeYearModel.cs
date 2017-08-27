using System;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Extensions;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSummarizeMonthsModel : BaseAccountsModel
    {
        public ReportSummarizeMonthsModel(Int16? id)
        {
            var currentYear = (Int16)Today.Year;
            
            var year = id ?? currentYear;
            
            year = year.ForceBetween(1900, currentYear);

            Year = Report.GetYearReport(Account.Name, year);

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