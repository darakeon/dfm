using System;
using System.Collections.Generic;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class YearJson
    {
        public Int32 ID { get; set; }

        public Int16 Time { get; set; }
        public String Account { get; set; }

        public IList<MonthJson> MonthList { get; set; }
        public IList<SummaryJson> SummaryList { get; set; }

        

    }
}
