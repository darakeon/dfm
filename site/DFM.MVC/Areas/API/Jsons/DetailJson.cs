using System;
using DFM.Entities;

namespace DFM.MVC.Areas.API.Jsons
{
    public class DetailJson
    {
        public String Description { get; set; }
        public Int16 Amount { get; set; }
        public Double Value { get; set; }

        public Detail ConvertToEntity()
        {
            return new Detail
            {
                Amount = Amount,
                Value = Value,
                Description = Description
            };
        }
    }
}