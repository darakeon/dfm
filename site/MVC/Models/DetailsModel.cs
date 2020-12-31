using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class DetailsModel
	{
		public DetailsModel(MoveInfo move, String language)
		{
			Guid = move.Guid;
			Description = move.Description;
			Value = move.Value;
			Language = language;
			DetailList = move.DetailList;
		}

		public Guid Guid { get; }
		public String Description { get; }
		public Decimal Value { get; }
		public String Language { get; }
		public IList<DetailInfo> DetailList { get; }
	}
}
