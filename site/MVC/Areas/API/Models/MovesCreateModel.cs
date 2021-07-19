using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Api.Helpers;

namespace DFM.MVC.Areas.Api.Models
{
	internal class MovesCreateModel : BaseApiModel
	{
		public MovesCreateModel(Guid? guid = null)
		{
			IsUsingCategories = isUsingCategories;

			if (guid.HasValue && guid != Guid.Empty)
			{
				Move = money.GetMove(guid.Value);
			}
		}

		public Boolean IsUsingCategories { get; }
		public MoveInfo Move { get; set; }

		public void Save(MoveInfo info)
		{
			money.SaveMove(info);
		}
	}
}
