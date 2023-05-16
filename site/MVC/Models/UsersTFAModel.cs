using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class UsersTFAModel : BaseSiteModel
	{
		public String Code { get; set; }

		public void Validate(Action<String, String> addModelError)
		{
			try
			{
				auth.ValidateTicketTFA(Code);
			}
			catch (CoreError exception)
			{
				addModelError("Code",
					translator[exception]
				);
			}
		}
	}
}
