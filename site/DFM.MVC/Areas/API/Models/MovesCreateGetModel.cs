using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.MVC.Areas.API.Helpers;

namespace DFM.MVC.Areas.API.Models
{
	internal class MovesCreateGetModel : BaseApiModel
	{
		public MovesCreateGetModel(Int32? id)
		{
			IsUsingCategories = isUsingCategories;

			if (id.HasValue && id != 0)
			{
				var move = money.GetMoveById(id.Value);

				Move = new MovesCreatePostModel(move);
			}

			AccountList = admin.GetAccountList(true)
				.Select(a => new SelectItem<String, String>(a.Name, a.Url))
				.ToList();

			if (isUsingCategories)
			{
				CategoryList = admin.GetCategoryList(true)
					.Select(a => new SelectItem<String, String>(a.Name, a.Name))
					.ToList();
			}

			NatureList = AccountList.Count > 1
				? SelectItemEnum.SelectItem<MoveNature>()
				: SelectItemEnum.SelectItem<PrimalMoveNature>();
		}

		public Boolean IsUsingCategories { get; }

		public MovesCreatePostModel Move { get; set; }

		public IList<SelectItem<String, String>> AccountList { get; set; }
		public IList<SelectItem<String, String>> CategoryList { get; set; }
		public IList<SelectItem<String, Int32>> NatureList { get; set; }

	}
}