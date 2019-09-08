using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.InterfacesAndBases
{
	public interface IPasswordForm
	{
		String Password { get; set; }
		String RetypePassword { get; set; }
	}

	internal static class PasswordFormExtension
	{
		public static void Verify(this IPasswordForm passwordForm)
		{
			if (String.IsNullOrEmpty(passwordForm.Password))
				throw Error.UserPasswordRequired.Throw();

			if (passwordForm.Password != passwordForm.RetypePassword)
				throw Error.RetypeWrong.Throw();
		}
	}

}
