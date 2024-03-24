using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.API.Models
{
	public class UsersLoginModel : BaseApiModel
	{
		[Required(ErrorMessage = "*")]
		public String Email { private get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { private get; set; }

		public Boolean RememberMe { private get; set; }

		public String Ticket { get; private set; }

		internal UsersLoginModel LogOn()
		{
			try
			{
				Ticket = current.Set(Email, Password, RememberMe);
				return this;
			}
			catch (CoreError e)
			{
				if (e.Type == Error.DisabledUser)
					outside.SendUserVerify(Email);

				throw;
			}
		}
	}
}
