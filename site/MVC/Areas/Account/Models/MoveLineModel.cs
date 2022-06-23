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
			Boolean canCheck,
			Boolean canHighlight)
		{
			Move = move;
			IsUsingCategories = isUsingCategories;
			CurrentAccountUrl = currentAccountUrl;
			Language = language;
			CanCheck = canCheck;
			CanHighlight = canHighlight;

			// to open modals of foreseen moves correctly
			if (Move.Guid == Guid.Empty)
				Move.Guid = Guid.NewGuid();
		}

		public MoveInfo Move { get; }
		public Boolean IsUsingCategories { get; }
		public String CurrentAccountUrl { get; }
		public String Language { get; }
		public Boolean CanCheck { get; }

		public Boolean CanHighlight { get; }
	}
}
