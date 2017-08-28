using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class UsersLogOnModel : BaseModel
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

		internal void LogOn()
		{
			try
			{
				Current.Set(Email, Password);
			}
			catch (DFMCoreException e)
			{
				if (e.Type == ExceptionPossibilities.DisabledUser)
				{
					var verifyError = sendUserVerify();

					if (verifyError != null)
					{
						throw verifyError;
					}
				}

				throw;
			}
		}

		private DFMCoreException sendUserVerify()
		{
			try
			{
				var pathAction = Url.Action("UserVerification", "Tokens");
				var pathDisable = Url.Action("Disable", "Tokens");
				Safe.SendUserVerify(Email, pathAction, pathDisable);
			}
			catch (DFMCoreException e)
			{
				return e;
			}

			return null;
		}
	}
}