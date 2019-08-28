using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Areas.API.Models
{
	internal class UsersLoginModel : BaseApiModel
	{
		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		public Boolean RememberMe { get; set; }

		internal String LogOn()
		{
			try
			{
				return login(Email, Password, RememberMe);
			}
			catch (DFMCoreException e)
			{
				if (e.Type == DfMError.DisabledUser)
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
				safe.SendUserVerify(Email);
			}
			catch (DFMCoreException e)
			{
				return e;
			}

			return null;
		}
	}
}