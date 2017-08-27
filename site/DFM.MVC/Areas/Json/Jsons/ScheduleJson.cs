using System;
using System.Collections.Generic;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class ScheduleJson
    {
        public String Description { get; set; }
        public MoveNature Nature { get; set; }
        public IList<DetailJson> DetailList { get; set; }
        
        public Boolean ShowInstallment { get; set; }
        
        public DateTime Date { get; set; }

        public Int16 LastRun { get; set; }
        public Int16 Deleted { get; set; }
        public Int16 Times { get; set; }

        public ScheduleFrequency Frequency { get; set; }
        public Boolean Boundless { get; set; }
        
        public Boolean Active { get; set; }
        

        public String Category { get; set; }
        public String AccountIn { get; set; }
        public String AccountOut { get; set; }


    }
}