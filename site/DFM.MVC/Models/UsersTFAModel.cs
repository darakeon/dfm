using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersTFAModel : BaseSiteModel
	{
		public String Code { get; set; }

		public void Validate(Action<String, String> addModelError)
		{
			try
			{
				safe.ValidateTicketTFA(Code);
			}
			catch (DFMCoreException exception)
			{
				addModelError("Code",
					MultiLanguage.Dictionary[exception]
				);
			}
		}
	}
}