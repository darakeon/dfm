using System;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Helpers;

namespace DFM.MVC.Areas.API.Jsons
{
    internal class SimpleMoveJson
    {
        public Int64 ID { get; set; }
        
        public String Description { get; set; }
        public DFMDate Date { get; set; }
        public MoveNature Nature { get; set; }

        public Double Total { get; set; }

        public SimpleMoveJson(Move move, String accountUrl)
        {
            ID = move.ID;

            Description = move.Description;
            Date = new DFMDate(move.Date);

            var accountOut = move.Nature != MoveNature.In ? move.AccOut().Url : null;

            Total = move.Total() * (accountUrl == accountOut ? -1 : 1);
        }
        
    }
}
