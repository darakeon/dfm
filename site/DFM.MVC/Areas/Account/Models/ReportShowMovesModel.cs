using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Helpers.Models;

namespace DFM.MVC.Areas.Account.Models
{
    public class ReportShowMovesModel : BaseAccountsModel
    {
        public ReportShowMovesModel(Int32? id)
        {
            var dateMonth = DateFromInt.GetDateMonth(id, Today);
            var dateYear = DateFromInt.GetDateYear(id, Today);

            MoveList = Report.GetMonthReport(CurrentAccountUrl, dateMonth, dateYear);
            
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