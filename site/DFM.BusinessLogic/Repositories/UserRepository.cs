using System;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class UserRepository : BaseRepositoryLong<User>
	{
		private const string emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";

		internal User GetByEmail(String email)
		{
			return SingleOrDefault(u => u.Email == email);
		}

		internal User ValidateAndGet(String email, String password)
		{
			var user = GetByEmail(email);

			if (user == null || !Crypt.Check(password, user.Password))
				throw Error.InvalidUser.Throw();

			if (!user.Active)
				throw Error.DisabledUser.Throw();

			return user;
		}

		internal User Save(User user)
		{
			if (user.ID != 0)
				throw Error.InvalidUser.Throw();

			return saveOrUpdate(user);
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

			return saveOrUpdate(user);
		}

		private User saveOrUpdate(User user)
		{
			return SaveOrUpdate(user, complete, validate);
		}

		private void validate(User user)
		{
			if (String.IsNullOrEmpty(user.Password))
				throw Error.UserPasswordRequired.Throw();

			if (user.Email.Length > MaxLen.User_Email)
				throw Error.TooLargeUserEmail.Throw();

			validateEmail(user);
		}

		private void validateEmail(User user)
		{
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

			user.Config.TimeZone = Defaults.CONFIG_TIMEZONE;

			user.Config.SendMoveEmail = Defaults.CONFIG_SEND_MOVE_EMAIL;
			user.Config.UseCategories = Defaults.CONFIG_USE_CATEGORIES;
			user.Config.MoveCheck = Defaults.CONFIG_MOVE_CHECK;

			user.Config.Theme = Defaults.DEFAULT_THEME;

			user.Creation = DateTime.UtcNow;
			user.Password = Crypt.Do(user.Password);
		}



		public Boolean VerifyPassword(User user, String password)
		{
			return Crypt.Check(password, user.Password);
		}

		public void SaveTFA(User user, String secret)
		{
			user.TFASecret = secret;
			update(user);
		}
	}
}
