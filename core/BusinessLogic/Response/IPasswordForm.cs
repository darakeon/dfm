using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Response
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

			if (passwordForm.Password.Length < Defaults.PasswordMinimumLength)
				throw Error.UserPasswordTooShort.Throw();

			if (passwordForm.Password.Length > Defaults.PasswordMaximumLength)
				throw Error.UserPasswordTooLong.Throw();

			if (passwordForm.Password != passwordForm.RetypePassword)
				throw Error.RetypeWrong.Throw();
		}
	}
}
