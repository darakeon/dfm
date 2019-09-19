using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Jsons;

namespace DFM.MVC.Areas.API.Models
{
	public class MovesCreatePostModel : BaseApiModel
	{
		public MovesCreatePostModel()
		{
			DetailList = new List<DetailJson>();
		}

		public MovesCreatePostModel(MoveInfo move) : this()
		{
			ID = move.ID;
			Description = move.Description;
			Date = new DateJson(move.Date);

			Category = move.CategoryName;

			Nature = move.Nature;
			AccountOutUrl = move.OutUrl;
			AccountInUrl = move.InUrl;

			Value = move.Value;
			DetailList = move.DetailList.Select(d => new DetailJson(d)).ToList();
		}

		public Int64 ID { get; set; }
		public String Description { get; set; }
		public DateJson Date { get; set; }
		public String Category { get; set; }
		public MoveNature Nature { get; set; }
		public String AccountOutUrl { get; set; }
		public String AccountInUrl { get; set; }
		public Decimal? Value { get; set; }

		public IList<DetailJson> DetailList { get; set; }

		internal void Save()
		{
			var move = convertToEntity();

			money.SaveMove(move);
		}

		private MoveInfo convertToEntity()
		{
			return new MoveInfo
			{
				ID = ID,
				Description = Description,
				Date = Date.ToSystemDate(),

				CategoryName = Category,

				Nature = Nature,
				InUrl = AccountInUrl,
				OutUrl = AccountOutUrl,

				Value = Value,
				DetailList = DetailList.Select(d => d.ConvertToEntity()).ToList(),
			};
		}
	}
}
