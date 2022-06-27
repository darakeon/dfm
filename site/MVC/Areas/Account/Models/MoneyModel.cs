using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoneyModel : BaseAccountModel
	{
		public void DeleteMove(Guid guid)
		{
			var move = getMove(guid);

			if (move == null)
			{
				errorAlert.Add("MoveNotFound");
				return;
			}

			var result = money.DeleteMove(guid);

			if (result.Email.IsWrong())
			{
				var deleted = translator["MoveDeletedWithoutEmail"];
				var error = translator[result.Email];
				var message = String.Format(deleted, move.Description, error);
				errorAlert.AddTranslated(message);
			}
			else
			{
				var deleted = translator["MoveDeleted"];
				var message = String.Format(deleted, move.Description);
				errorAlert.AddTranslated(message);
			}

			ReportUrl = move.ToMonthYear();
		}

		public MoveLineModel CheckMove(Guid guid, PrimalMoveNature nature)
		{
			return toggleCheck(guid, nature, money.CheckMove);
		}

		public MoveLineModel UncheckMove(Guid guid, PrimalMoveNature nature)
		{
			return toggleCheck(guid, nature, money.UncheckMove);
		}

		private MoveLineModel toggleCheck(
			Guid guid,
			PrimalMoveNature nature,
			Func<Guid, PrimalMoveNature, MoveInfo> toggleCheck
		)
		{
			return new(
				tryToggleCheck(guid, nature, toggleCheck),
				isUsingCategories,
				CurrentAccountUrl,
				language,
				moveCheckingEnabled
			);
		}

		private MoveInfo tryToggleCheck(Guid guid, PrimalMoveNature nature, Func<Guid, PrimalMoveNature, MoveInfo> toggleCheck)
		{
			try
			{
				return toggleCheck(guid, nature);
			}
			catch (CoreError e)
			{
				if (e.Type == Error.MoveAlreadyChecked || e.Type == Error.MoveAlreadyUnchecked)
					return getMove(guid);

				throw;
			}
		}

		private MoveInfo getMove(Guid guid)
		{
			try
			{
				return money.GetMove(guid);
			}
			catch (CoreError)
			{
				return null;
			}
		}

		public Int32? ReportUrl;
	}
}
