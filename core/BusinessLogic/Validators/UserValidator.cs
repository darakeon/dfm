using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using Keon.Util.Crypto;

namespace DFM.BusinessLogic.Validators;

internal class UserValidator
{
	public Boolean VerifyPassword(User user, String password)
	{
		return password != null
			&& Crypt.Check(password, user.Password);
	}

	public void CheckPassword(User user, String password)
	{
		if (!VerifyPassword(user, password))
			throw Error.WrongPassword.Throw();
	}

	public void CheckUserDeletion(User user)
	{
		if (user.Control.ProcessingDeletion)
			throw Error.UserDeleted.Throw();

		if (user.Control.WipeRequest != null)
			throw Error.UserAskedWipe.Throw();
	}
}
