using System;
using DFM.Entities;
using DFM.MVC.Models;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSummarizeMonthsModel : BaseLoggedModel
    {
        public Year Year { get; set; }

        public String Date
        {
            get
            {
                return String.Format(PlainText.Dictionary["ShortDateFormat"],
                                     PlainText.Dictionary["Summary"], Year);
            }
        }
    }
}