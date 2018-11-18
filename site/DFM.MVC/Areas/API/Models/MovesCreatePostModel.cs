using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
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

		public MovesCreatePostModel(Move move) : this()
		{
			ID = move.ID;
			Description = move.Description;
			Date = new DateJson(move.Date);
			Category = move.Category?.Name;
			Nature = move.Nature;
			AccountOutUrl = move.AccOut()?.Url;
			AccountInUrl = move.AccIn()?.Url;
			Value = move.Value;

			DetailList = move.DetailList.Select(d => new DetailJson(d)).ToList();
		}

		public Int32 ID { get; set; }
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

			money.SaveOrUpdateMove(move, AccountOutUrl, AccountInUrl, Category);
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