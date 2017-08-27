using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Jsons;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Models
{
    public class MoveCreatePostModel : BaseApiModel
    {
        public MoveCreatePostModel()
        {
            DetailList = new List<DetailJson>();
        }

        public String Description { get; set; }
        public DateTime Date { get; set; }
        public String Category { get; set; }
        public MoveNature Nature { get; set; }
        public String PrimaryAccount { get; set; }
        public String OtherAccount { get; set; }
        public Double Value { get; set; }

        public IList<DetailJson> DetailList { get; set; }

        internal void Save()
        {
            var move = convertToEntity();

            var accountSelector = new AccountSelector(Nature, PrimaryAccount, OtherAccount);

            Money.SaveOrUpdateMove(move, accountSelector.AccountOutName, accountSelector.AccountInName, Category);
        }

        private Move convertToEntity()
        {
            if (!DetailList.Any())
            {
                //TODO: replace by Value
                var detail = new DetailJson
                {
                    Amount = 1,
                    Value = Value,
                    Description = Description
                };

                DetailList.Add(detail);
            }

            return new Move
            {
                Description = Description,
                Date = Date,
                Nature = Nature,
                DetailList = DetailList.Select(d => d.ConvertToEntity()).ToList()
            };
        }
    }
}