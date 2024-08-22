using System;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Views;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoveLineModel
	{
		public MoveLineModel(
			MoveInfo move,
			Boolean isUsingCategories,
			String currentAccountUrl,
			String language,
			Boolean canCheck
		)
		{
			Move = move;
			IsUsingCategories = isUsingCategories;
			CurrentAccountUrl = currentAccountUrl;
			Language = language;
			CanCheck = canCheck;

			// to open modals of foreseen moves correctly
			if (Move.Guid == Guid.Empty)
				Move.Guid = Guid.NewGuid();
		}

		public MoveInfo Move { get; }
		public Boolean IsUsingCategories { get; }
		public String CurrentAccountUrl { get; }
		public String Language { get; }
		public Boolean CanCheck { get; }

		# nullable enable
		public WizardHL? WizardHL { get; set; }
	}
}
