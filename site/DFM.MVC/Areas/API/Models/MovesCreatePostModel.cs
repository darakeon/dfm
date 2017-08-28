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

	    public MovesCreatePostModel(Move move) : this()
	    {
		    ID = move.ID;
			Description = move.Description;
			Date = new DateJson(move.Date);
			Category = move.Category == null ? null : move.Category.Name;
			Nature = move.Nature;
			OtherAccount = move.Nature == MoveNature.Transfer ? move.AccIn().Name : null;

			if (move.Value.HasValue)
				Value = move.Value.Value;

			DetailList = move.DetailList.Select(d => new DetailJson(d)).ToList();
	    }

		public Int32 ID { get; set; }
		public String Description { get; set; }
		public DateJson Date { get; set; }
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
				ID = ID,
                Description = Description,
                Date = Date.ToSystemDate(),
                Nature = Nature,
				Value = Value,
                DetailList = DetailList.Select(d => d.ConvertToEntity()).ToList(),
            };
        }
    }
}