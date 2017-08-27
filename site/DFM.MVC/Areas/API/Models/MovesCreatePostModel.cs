using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Models
{
    public class MovesCreatePostModel : BaseApiModel
    {
        public MovesCreatePostModel()
        {
            DetailList = new List<DetailJson>();
        }

        public String Description { get; set; }
        public DateTime Date { get; set; }
        public String Category { get; set; }
        public MoveNature Nature { get; set; }
        public String PrimaryAccount { get; set; }
        public String OtherAccount { get; set; }
        public Decimal Value { get; set; }

        public IList<DetailJson> DetailList { get; set; }

        internal void Save()
        {
            var move = convertToEntity();

            var accountSelector = new AccountSelector(Nature, PrimaryAccount, OtherAccount);

            Money.SaveOrUpdateMove(move, accountSelector.AccountOutUrl, accountSelector.AccountInUrl, Category);
        }

        private Move convertToEntity()
        {
            return new Move
            {
                Description = Description,
                Date = Date,
                Nature = Nature,
				Value = Value,
                DetailList = DetailList.Select(d => d.ConvertToEntity()).ToList(),
            };
        }
    }
}