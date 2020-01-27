using System;
using DFM.BusinessLogic.Exceptions;

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
					translator[exception]
				);
			}
		}
	}
}
