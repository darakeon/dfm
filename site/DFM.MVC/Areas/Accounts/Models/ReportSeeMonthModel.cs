using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Extensions;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportShowMovesModel : BaseAccountsModel
    {
        public ReportShowMovesModel(Int32? id)
        {
            var currentMonth = (Int16)Today.Month;

            var dateMonth = id.HasValue
                ? (Int16)(id.Value % 100)
                : currentMonth;

            dateMonth = dateMonth.ForceBetween(1, 12);


            var currentYear = (Int16)Today.Year;

            var dateYear = id.HasValue
                ? (Int16)(id.Value / 100)
                : currentYear;

            dateYear = dateYear.ForceBetween(1900, currentYear);

            MoveList = Report.GetMonthReport(AccountUrl, dateMonth, dateYear);
            
            Month = dateMonth;
            Year = dateYear;

        }



        public IList<Move> MoveList { get; set; }
        
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }

        public String Date
        {
            get
            {
                return String.Format(MultiLanguage.Dictionary["ShortDateFormat"],
                                     MultiLanguage.GetMonthName(Month), Year);
            }
        }
    }
}