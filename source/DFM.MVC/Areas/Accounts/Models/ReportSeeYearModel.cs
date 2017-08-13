using System;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSummarizeMonthsModel : BaseLoggedModel
    {
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