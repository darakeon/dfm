using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
    public class DetailAddModel : BaseModel
    {
        public DetailAddModel()
        {
            DetailList = new List<Detail>();
        }

        public DetailAddModel(Int32 position)
            : this()
        {
            Position = position;

            for (var d = 0; d <= position; d++)
            {
                DetailList.Add(new Detail());
            }
        }

        public DetailAddModel(Detail detail, Int32 position)
            : this(position)
        {
            DetailList[position] = detail;
        }

        public Int32 Position { get; set; }
        public IList<Detail> DetailList { get; set; }


    }
}