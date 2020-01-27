using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoveLineModel
	{
		public MoveLineModel(
			MoveInfo move,
			Boolean isUsingCategories,
			String currentAccountUrl,
			String language,
			Boolean canCheck)
		{
			Move = move;
			IsUsingCategories = isUsingCategories;
			CurrentAccountUrl = currentAccountUrl;
			Language = language;
			CanCheck = canCheck;
		}

		public MoveInfo Move { get; }
		public Boolean IsUsingCategories { get; }
		public String CurrentAccountUrl { get; }
		public String Language { get; }
		public Boolean CanCheck { get; }
	}
}
