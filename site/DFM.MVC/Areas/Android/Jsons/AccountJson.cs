using System;
using DFM.Entities;

namespace DFM.MVC.Areas.Android.Jsons
{
    internal class AccountJson
    {
        public String Name { get; set; }
        public String Url { get; set; }

        public Double? RedLimit { get; set; }
        public Double? YellowLimit { get; set; }
        //public DateTime BeginDate { get; set; }
        //public DateTime? EndDate { get; set; }

        public AccountJson(Account account)
        {
            Name = account.Name;
            Url = account.Url;
            RedLimit = account.RedLimit;
            YellowLimit = account.YellowLimit;
            //BeginDate = account.BeginDate;
            //EndDate = account.EndDate;
        }



    }
}
