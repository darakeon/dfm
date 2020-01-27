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
		public MovesCreateModel(Int32? id = null)
		{
			IsUsingCategories = isUsingCategories;

			if (id.HasValue && id != 0)
			{
				Move = money.GetMove(id.Value);
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
				? SelectItemEnum.SelectItem<MoveNature>(translator, service)
				: SelectItemEnum.SelectItem<PrimalMoveNature>(translator, service);
		}

		public Boolean IsUsingCategories { get; }

		public MoveInfo Move { get; set; }

		public IList<SelectItem<String, String>> AccountList { get; set; }
		public IList<SelectItem<String, String>> CategoryList { get; set; }
		public IList<SelectItem<String, Int32>> NatureList { get; set; }

		public void Save(MoveInfo info)
		{
			money.SaveMove(info);
		}
	}
}
