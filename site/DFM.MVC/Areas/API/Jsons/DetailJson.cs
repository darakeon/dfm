using System;
using DFM.Entities;

namespace DFM.MVC.Areas.API.Jsons
{
    internal class DetailJson
    {
        public Int64 FakeID { get; set; }
        
        public String Description { get; set; }
        public Int16 Amount { get; set; }
        public Double Value { get; set; }

        public DetailJson(Detail detail)
        {
            FakeID = detail.FakeID;

            Description = detail.Description;
            Amount = detail.Amount;
            Value = detail.Value;
        }

    }
}
