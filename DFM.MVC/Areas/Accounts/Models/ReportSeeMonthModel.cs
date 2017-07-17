using System;
using System.Collections.Generic;
using DFM.Core.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSeeMonthModel : BaseLoggedModel
    {
        public ReportSeeMonthModel()
        {
            MoveList = new List<Move>();
        }

        public IList<Move> MoveList { get; set; }
        public Account Account { get; set; }
        
        public Int32 Year { get; set; }
        public String Month { get; set; }
    }
}