using System;
using System.Collections.Generic;
using DFM.Core.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class ReportSeeYearModel : BaseLoggedModel
    {
        public ReportSeeYearModel()
        {
            MoveSumList = new Dictionary<String, Double>();
        }

        public IDictionary<String, Double> MoveSumList;
        public Account Account { get; set; }

        public Int32 Year { get; set; }
    }
}