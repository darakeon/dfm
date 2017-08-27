using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class SummaryJson
    {
        public Int32 ID { get; set; }

        public Double In { get; set; }
        public Double Out { get; set; }
        public SummaryNature Nature { get; set; }

        public Boolean Broken { get; set; }

        public CategoryJson Category { get; set; }
        public MonthJson Month { get; set; }
        public YearJson Year { get; set; }


    }
}
