using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities.Bases;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Account.Models
{
	public class MoneyModel : BaseSiteModel
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

		public void CheckMove(int id)
		{
			try
			{
				var move = money.CheckMove(id);
				ReportUrl = move.MonthYear;
			}
			catch (CoreError e)
			{
				ErrorAlert.Add(e.Type);
			}
		}

		public void UncheckMove(int id)
		{
			try
			{
				var move = money.UncheckMove(id);
				ReportUrl = move.MonthYear;
			}
			catch (CoreError e)
			{
				ErrorAlert.Add(e.Type);
			}
		}

		private MoveInfo getMove(int id)
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
