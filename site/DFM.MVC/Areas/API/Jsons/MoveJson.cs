using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.API.Jsons
{
    internal class MoveJson
    {
        public Int64 FakeID { get; set; }
        
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public MoveNature Nature { get; set; }

        public String Category { get; set; }
        public String AccountIn { get; set; }
        public String AccountOut { get; set; }

        public Double Total { get; set; }

        public IList<DetailJson> DetailList { get; set; }

        public MoveJson(Move move)
        {
            FakeID = move.FakeID;

            Description = move.Description;
            Date = move.Date;
            Nature = move.Nature;

            Category = move.Category.Name;
            AccountIn = move.Nature != MoveNature.Out ? move.AccIn().Name : null;
            AccountOut = move.Nature != MoveNature.In ? move.AccOut().Name : null;

            Total = move.Value();

            DetailList = move.DetailList
                .Select(d => new DetailJson(d))
                .ToList();
        }
        
    }
}
