using System;
using System.Collections.Generic;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Json.Jsons
{
    internal class MoveJson
    {
        public Int32 FakeID { get; set; }
        
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public MoveNature Nature { get; set; }

        public String Category { get; set; }
        public String AccountIn { get; set; }
        public String AccountOut { get; set; }

        public IList<DetailJson> DetailList { get; set; }

        
    }
}
