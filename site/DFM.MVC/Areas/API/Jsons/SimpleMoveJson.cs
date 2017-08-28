using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.API.Jsons
{
    internal class SimpleMoveJson
    {
        public Int64 ID { get; set; }
        
        public String Description { get; set; }
        public DateJson Date { get; set; }
        public MoveNature Nature { get; set; }

        public Decimal Total { get; set; }

        public SimpleMoveJson(Move move, String accountUrl)
        {
            ID = move.ID;

            Description = move.Description;
            Date = new DateJson(move.Date);

            var accountOut = move.Nature != MoveNature.In ? move.AccOut().Url : null;

            Total = move.Total() * (accountUrl == accountOut ? -1 : 1);
        }
        
    }
}
