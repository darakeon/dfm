using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class UsersVerificationModel : BaseSiteModel
	{
		public void Send(Action<String, String> addModelError)
		{
			try
			{
				outside.SendUserVerify(current.Email);
			}
			catch (CoreError exception)
			{
				addModelError("", translator[exception]);
			}
		}
	}
}