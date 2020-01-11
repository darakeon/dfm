using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoneyModel : BaseAccountModel
	{
		public void DeleteMove(Int32 id)
		{
			var move = getMove(id);

			if (move == null)
			{
				ErrorAlert.Add("MoveNotFound");
				return;
			}

			var result = money.DeleteMove(id);

			if (result.Email.IsWrong())
			{
				var deleted = Translator.Dictionary["MoveDeletedWithoutEmail"];
				var error = Translator.Dictionary[result.Email];
				var message = String.Format(deleted, move.Description, error);
				ErrorAlert.AddTranslated(message);
			}
			else
			{
				var deleted = Translator.Dictionary["MoveDeleted"];
				var message = String.Format(deleted, move.Description);
				ErrorAlert.AddTranslated(message);
			}

			ReportUrl = move.ToMonthYear();
		}

		public MoveLineModel CheckMove(Int32 id, PrimalMoveNature nature)
		{
			return toggleCheck(id, nature, money.CheckMove);
		}

		public MoveLineModel UncheckMove(Int32 id, PrimalMoveNature nature)
		{
			return toggleCheck(id, nature, money.UncheckMove);
		}

		private MoveLineModel toggleCheck(Int64 id, PrimalMoveNature nature, Func<Int64, PrimalMoveNature, MoveInfo> toggleCheck)
		{
			return new MoveLineModel(
				tryToggleCheck(id, nature, toggleCheck),
				isUsingCategories,
				CurrentAccountUrl,
				language,
				moveCheckingEnabled
			);
		}

		private MoveInfo tryToggleCheck(Int64 id, PrimalMoveNature nature, Func<Int64, PrimalMoveNature, MoveInfo> toggleCheck)
		{
			try
			{
				return toggleCheck(id, nature);
			}
			catch (CoreError e)
			{
				if (e.Type == Error.MoveAlreadyChecked || e.Type == Error.MoveAlreadyUnchecked)
					return getMove(id);

				throw;
			}
		}

		private MoveInfo getMove(Int64 id)
		{
			try
			{
				return money.GetMove(id);
			}
			catch (CoreError)
			{
				return null;
			}
		}

		public Int32? ReportUrl;
	}
}
