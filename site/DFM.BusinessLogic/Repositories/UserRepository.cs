using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Repositories
{
	internal class UserRepository : BaseRepository<User>
	{
		private const string email_pattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";
		
		
		
		internal User GetByEmail(String email)
		{
			return SingleOrDefault(u => u.Email == email);
		}



		internal User ValidateAndGet(String email, String password)
		{
			var user = GetByEmail(email);
			password = encrypt(password);

			if (user == null || user.Password != password)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

			if (!user.Active)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledUser);

			return user;
		}



		internal User Save(User user)
		{
			if (user.ID != 0)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

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
			user.Password = encrypt(user.Password);

			return update(user);
		}


		internal User UpdateEmail(Int32 id, String email)
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
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

			return saveOrUpdate(user);
		}




		private User saveOrUpdate(User user)
		{
			return SaveOrUpdate(user, complete, validate);
		}

		private void validate(User user)
		{
			if (String.IsNullOrEmpty(user.Password))
				throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

			validateEmail(user);
		}

		private void validateEmail(User user)
		{
			var regex = new Regex(email_pattern, RegexOptions.IgnoreCase);

			if (!regex.Match(user.Email).Success)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.UserEmailInvalid);

			var userByEmail = GetByEmail(user.Email);

			if (userByEmail != null && userByEmail.ID != user.ID)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.UserAlreadyExists);
		}

		private void complete(User user)
		{
			var oldUser = GetByEmail(user.Email);
			
			var userIsNew = oldUser == null;

			if (!userIsNew) return;

			user.Active = false;
			
			user.Config.TimeZone = Defaults.CONFIG_TIMEZONE;
			
			user.Config.SendMoveEmail = Defaults.CONFIG_SEND_MOVE_EMAIL;
			user.Config.UseCategories = Defaults.CONFIG_USE_CATEGORIES;
			user.Config.MoveCheck = Defaults.CONFIG_MOVE_CHECK;

			user.Config.Theme = Defaults.DEFAULT_THEME;

			user.Creation = DateTime.UtcNow;
			user.Password = encrypt(user.Password);
		}



		private static String encrypt(String password)
		{
			if (String.IsNullOrEmpty(password))
				return null;

			var md5 = new MD5CryptoServiceProvider();

			var originalBytes = Encoding.Default.GetBytes(password);
			var encodedBytes = md5.ComputeHash(originalBytes);

			var hexCode = BitConverter
							.ToString(encodedBytes)
							.Replace("-", "");

			return hexCode;
		}



		public Boolean VerifyPassword(User user, String password)
		{
			return user.Password == encrypt(password);
		}

	}
}
