﻿using System;
using System.Text.RegularExpressions;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;
using Keon.TwoFactorAuth;
using Keon.Util.Crypto;

namespace DFM.BusinessLogic.Repositories
{
	internal class UserRepository : Repo<User>
	{
		private const string emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";

		internal User GetByEmail(String email)
		{
			return SingleOrDefault(u => u.Email == email);
		}

		internal Authentication ValidateAndGet(String email, String password)
		{
			var user = GetByEmail(email);

			if (user == null)
				throw Error.InvalidUser.Throw();

			var validPass = Crypt.Check(password, user.Password);
			var validCode = user.TFAPassword && IsValid(user.TFASecret, password);

			if (!validPass && !validCode)
				throw Error.InvalidUser.Throw();

			if (!user.Active)
				throw Error.DisabledUser.Throw();

			return new Authentication(user, validCode);
		}

		internal User Save(User user)
		{
			if (user.ID != 0)
				throw Error.InvalidUser.Throw();

			return save(user);
		}

		internal void Activate(User user)
		{
			user.Active = true;
			user.WrongLogin = 0;

			update(user);
		}

		internal User ChangePassword(User user)
		{
			user.Password = Crypt.Do(user.Password);

			return update(user);
		}

		internal User UpdateEmail(Int64 id, String email)
		{
			var user = Get(id);
			user.Email = email;
			user.Active = false;

			validateEmail(user);

			return update(user);
		}

		private User update(User user)
		{
			if (user.ID == 0)
				throw Error.InvalidUser.Throw();

			return save(user);
		}

		private User save(User user)
		{
			return SaveOrUpdate(user, complete, validate);
		}

		private void validate(User user)
		{
			if (String.IsNullOrEmpty(user.Password))
				throw Error.UserPasswordRequired.Throw();

			validateEmail(user);
		}

		private void validateEmail(User user)
		{
			if (String.IsNullOrEmpty(user.Email))
				throw Error.UserEmailRequired.Throw();

			if (user.Email.Length > MaxLen.UserEmail)
				throw Error.TooLargeUserEmail.Throw();

			var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

			if (!regex.Match(user.Email).Success)
				throw Error.UserEmailInvalid.Throw();

			var userByEmail = GetByEmail(user.Email);

			if (userByEmail != null && userByEmail.ID != user.ID)
				throw Error.UserAlreadyExists.Throw();
		}

		private void complete(User user)
		{
			if (user.ID != 0) return;

			user.Active = false;

			if (user.Config.Language == null)
				user.Config.Language = Defaults.ConfigLanguage;

			if (user.Config.TimeZone == null)
				user.Config.TimeZone = Defaults.ConfigTimeZone;

			user.Config.SendMoveEmail = Defaults.ConfigSendMoveEmail;
			user.Config.UseCategories = Defaults.ConfigUseCategories;
			user.Config.MoveCheck = Defaults.ConfigMoveCheck;

			user.Config.Theme = Defaults.DefaultTheme;

			user.Creation = DateTime.UtcNow;
			user.Password = Crypt.Do(user.Password);
		}



		public Boolean VerifyPassword(User user, String password)
		{
			return password != null
			    && Crypt.Check(password, user.Password);
		}

		public void SaveTFA(User user, String secret)
		{
			user.TFASecret = secret;
			update(user);
		}

		public void UseTFAAsPassword(User user, Boolean use)
		{
			user.TFAPassword = use;
			update(user);
		}

		public Boolean IsValid(String secret, String code)
		{
			return CodeGenerator
				.Generate(secret, 2)
				.Contains(code);
		}
	}
}
