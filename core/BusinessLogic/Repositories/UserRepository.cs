using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories.DataObjects;
using DFM.BusinessLogic.Validators;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Logs;
using Keon.Util.Crypto;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class UserRepository(Current.GetUrl getUrl, ILogService logService)
		: Repo<User>
	{
		private const String emailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";

		private readonly UserValidator validator = new();

		internal User GetByEmail(String email)
		{
			return SingleOrDefault(User.Compare(email));
		}

		internal UserTFA ValidateAndGet(String email, String password)
		{
			if (password == null)
				throw Error.InvalidUser.Throw();

			var user = GetByEmail(email);

			if (user == null)
				throw Error.InvalidUser.Throw();

			var validPass = validator.VerifyPassword(user, password);
			var validCode = user.TFAPassword && validator.VerifyTFA(user.TFASecret, password);

			if (!validPass && !validCode)
				throw Error.InvalidUser.Throw();

			if (!user.Control.ActiveOrAllowedPeriod())
				throw Error.DisabledUser.Throw();

			return new UserTFA(user, validCode);
		}

		internal User Save(User user)
		{
			if (user.ID != 0)
				throw Error.InvalidUser.Throw();

			return save(user);
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

			if (user.Username.Length > MaxLen.UserEmailUsername || user.Domain.Length > MaxLen.UserEmailDomain)
				throw Error.TooLargeUserEmail.Throw();

			var regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

			if (!regex.IsMatch(user.Email))
				throw Error.UserEmailInvalid.Throw();

			var userByEmail = GetByEmail(user.Email);

			if (userByEmail != null && userByEmail.ID != user.ID)
				throw Error.UserAlreadyExists.Throw();
		}

		private void complete(User user)
		{
			if (user.ID != 0) return;

			user.Settings.Language ??= Defaults.SettingsLanguage;
			user.Settings.TimeZone ??= Defaults.SettingsTimeZone;
			user.Settings.SendMoveEmail = Defaults.SettingsSendMoveEmail;
			user.Settings.UseCategories = Defaults.SettingsUseCategories;
			user.Settings.MoveCheck = Defaults.SettingsMoveCheck;
			user.Settings.UseAccountsSigns = Defaults.SettingsUseAccountsSigns;
			user.Settings.UseCurrency = Defaults.SettingsUseCurrency;

			user.Settings.Theme = Defaults.DefaultTheme;

			user.Password = Crypt.Do(user.Password);

			user.SetRobotCheckDay();
		}


		public void ClearTFA(User user, Boolean sendEmail)
		{
			if (user.TFAPassword)
				user.TFAPassword = false;

			SaveTFA(user, null);

			if (sendEmail)
			{
				sendSecurityWarning(user, SecurityWarning.TFARemoval);
			}
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

		public IList<User> GetForRunSchedule()
		{
			return NewQuery()
				.Where(u => u.Control, c => c.RobotCheck <= DateTime.UtcNow)
				.Where(u => u.Control, c => c.Active)
				.Where(u => u.Control, c => c.IsRobot == false)
				.List;
		}

		private void sendSecurityWarning(User user, SecurityWarning securityWarning)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
			};

			var format = Format.SecurityWarning(user, securityWarning);
			var fileContent = format.Layout.Format(dic);

			var sender = new Sender(logService)
				.To(user.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
			}
			catch (MailError)
			{
				throw Error.FailOnEmailSend.Throw();
			}
		}
	}
}
