using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersRemoveTFAModel : BaseSiteModel
	{
		public String Password { get; set; }

		public void Remove(Action<String, String> addModelError)
		{
			try
			{
				safe.RemoveTFA(Password);
			}
			catch (CoreError exception)
			{
				addModelError("Password",
					Translator.Dictionary[exception]
				);
			}
		}
	}
}