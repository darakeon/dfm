using System;
using System.Collections.Generic;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Jsons;

namespace DFM.MVC.Areas.API.Models
{
    public class MoveCreatePostModel
    {
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public String Category { get; set; }
        public MoveNature Nature { get; set; }
        public Double Value { get; set; }

        public IList<DetailJson> DetailList { get; set; }
    }
}