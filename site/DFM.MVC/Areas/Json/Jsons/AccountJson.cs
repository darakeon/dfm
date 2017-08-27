using System;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class AccountJson
    {
        public String Name { get; set; }
        public String Url { get; set; }

        public Double? RedLimit { get; set; }
        public Double? YellowLimit { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }



    }
}
