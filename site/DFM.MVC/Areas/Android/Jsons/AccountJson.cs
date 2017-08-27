using System;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.MVC.Areas.Android.Jsons
{
    internal class AccountJson
    {
        public String Name { get; set; }
        public String Url { get; set; }

        public Double Sum { get; set; }
        public Double? RedLimit { get; set; }
        public Double? YellowLimit { get; set; }

        public AccountJson(Account account)
        {
            Name = account.Name;
            Url = account.Url;
            Sum = account.Sum();
            RedLimit = account.RedLimit;
            YellowLimit = account.YellowLimit;
        }



    }
}
