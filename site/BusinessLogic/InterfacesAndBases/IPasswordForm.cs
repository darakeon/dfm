using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.InterfacesAndBases
{
	public interface IPasswordForm
	{
		String Password { get; }
		String RetypePassword { get; }
	}

	internal static class PasswordFormExtension
	{
		public static void VerifyPassword(this IPasswordForm passwordForm)
		{
			if (String.IsNullOrEmpty(passwordForm.Password))
				throw Error.UserPasswordRequired.Throw();

			if (passwordForm.Password != passwordForm.RetypePassword)
				throw Error.RetypeWrong.Throw();
		}
	}
}
