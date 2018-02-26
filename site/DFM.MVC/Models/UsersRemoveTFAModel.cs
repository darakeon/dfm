using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersRemoveTFAModel : BaseLoggedModel
	{
		public String Password { get; set; }

		public void Remove(Action<String, String> addModelError)
		{
			try
			{
				Safe.RemoveTFA(Password);
			}
			catch (DFMCoreException exception)
			{
				addModelError("Password",
					MultiLanguage.Dictionary[exception]
				);
			}
		}
	}
}