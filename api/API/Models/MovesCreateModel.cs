using System;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models
{
	internal class MovesCreateModel : BaseApiModel
	{
		public MovesCreateModel()
		{
			IsUsingCategories = isUsingCategories;
		}

		public MovesCreateModel(Guid guid) : this()
		{
			Move = money.GetMove(guid);
		}

		public bool IsUsingCategories { get; }
		public MoveInfo Move { get; set; }

		public void Save(MoveInfo info)
		{
			money.SaveMove(info);
		}
	}
}
