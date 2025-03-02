using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using Keon.TwoFactorAuth;
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

	public Boolean VerifyTFA(String secret, String code)
	{
		return CodeGenerator
			.Generate(secret, 2)
			.Contains(code);
	}

	public void CheckTFA(TFAInfo info)
	{
		checkTFA(info.Secret, info.TFACode);
	}

	public void CheckTFA(User user, String code)
	{
		checkTFA(user.TFASecret, code, user.Control.WrongTFA);
	}

	private void checkTFA(String secret, String code, Int32 wrongCount = 0)
	{
		if (VerifyTFA(secret, code))
			return;

		var error = // +1 because of the one that just happened
			wrongCount + 1 < Defaults.TFAErrorLimit
				? Error.TFAWrongCode
				: Error.TFATooMuchAttempt;

		throw error.Throw();
	}

	public void CheckTFAConfigured(User user)
	{
		if (!user.HasTFA())
			throw Error.TFANotConfigured.Throw();
	}
}
