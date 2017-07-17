using System;
using System.Collections.Generic;
using DFM.Core.Entities;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSeeMonthModel : BaseModel
    {
        public ReportSeeMonthModel()
        {
            MoveList = new List<Move>();
        }

        public IList<Move> MoveList { get; set; }
        public Int32 Year { get; set; }
        public String Month { get; set; }
    }
}