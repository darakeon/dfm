using System;
using System.Collections.Generic;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class MonthJson
    {
        public Int16 Time { get; set; }
        public Int16 Year { get; set; }
        public String Account { get; set; }

        public IList<SummaryJson> SummaryList { get; set; }
        public IList<MoveJson> InList { get; set; }
        public IList<MoveJson> OutList { get; set; }


    }
}