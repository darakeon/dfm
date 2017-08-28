using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoneyModel : BaseModel
	{
		public void DeleteMove(Int32 id)
		{
			var move = getMove(id);

			if (move == null)
			{
				ErrorAlert.Add("MoveNotFound");
				return;
			}
			
			var result = Money.DeleteMove(id);

			if (result.Error.IsWrong())
			{
				var deleted = MultiLanguage.Dictionary["MoveDeletedWithoutEmail"];
				var error = MultiLanguage.Dictionary[result.Error];
				var message = String.Format(deleted, move.Description, error);
				ErrorAlert.AddTranslated(message);
			}
			else
			{
				var deleted = MultiLanguage.Dictionary["MoveDeleted"];
				var message = String.Format(deleted, move.Description);
				ErrorAlert.AddTranslated(message);
			}

			ReportUrl = (move.Out ?? move.In).Url();
		}

		public void CheckMove(int id)
		{
			try
			{
				var move = Money.CheckMove(id);

				ReportUrl = (move.Out ?? move.In).Url();
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}

		public void UncheckMove(int id)
		{
			try
			{
				var move = Money.UncheckMove(id);

				ReportUrl = (move.Out ?? move.In).Url();
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}


		private Move getMove(int id)
		{
			try
			{
				return Money.GetMoveById(id);
			}
			catch (DFMCoreException)
			{
				return null;
			}
		}

		public Int32? ReportUrl;
	}

}