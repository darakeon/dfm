using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class UsersLogOnModel : BaseSiteModel
	{
		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		public Boolean RememberMe { get; set; }

		internal DFMCoreException TryLogOn()
		{
			try
			{
				LogOn();
				return null;
			}
			catch (DFMCoreException e)
			{
				return e;
			}
		}

		internal String LogOn()
		{
			return login(Email, Password, RememberMe);
		}
	}
}
