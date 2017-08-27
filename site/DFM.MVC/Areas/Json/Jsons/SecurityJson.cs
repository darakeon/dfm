using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Json.Jsons
{
    public class SecurityJson
    {
        public String Token { get; set; }
        public SecurityAction Action { get; set; }


    }
}
