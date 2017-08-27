using System;
using DFM.Entities;

namespace DFM.MVC.Areas.API.Jsons
{
    public class DetailJson
    {
        public String Description;
        public Int16 Amount;
        public Double Value;

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