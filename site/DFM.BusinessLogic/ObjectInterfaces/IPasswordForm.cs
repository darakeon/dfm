using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.ObjectInterfaces
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
				throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

			if (passwordForm.Password != passwordForm.RetypePassword)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.RetypeWrong);
		}
	}

}
