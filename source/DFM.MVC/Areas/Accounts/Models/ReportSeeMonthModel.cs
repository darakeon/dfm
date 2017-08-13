using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportShowMovesModel : BaseLoggedModel
    {
        public ReportShowMovesModel()
        {
            MoveList = new List<Move>();
        }

        public IList<Move> MoveList { get; set; }
        public Account Account { get; set; }
        
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