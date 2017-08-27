using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.API.Jsons
{
    internal class SimpleMoveJson
    {
        public Int64 FakeID { get; set; }
        
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public MoveNature Nature { get; set; }

        public Double Total { get; set; }

        public SimpleMoveJson(Move move, String accountUrl)
        {
            FakeID = move.FakeID;

            Description = move.Description;
            Date = move.Date;

            var accountOut = move.Nature != MoveNature.In ? move.AccOut().Url : null;

            Total = move.Value() * (accountUrl == accountOut ? -1 : 1);
        }
        
    }
}
