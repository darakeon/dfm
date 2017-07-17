using System;
using System.Collections.Generic;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSeeYearModel : BaseModel
    {
        public ReportSeeYearModel()
        {
            MoveSumList = new Dictionary<String, Double>();
        }

        public IDictionary<String, Double> MoveSumList;
        public Int32 Year { get; set; }
    }
}