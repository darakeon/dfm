using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DFM.Authentication;
using DK.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Multilanguage;
using DFM.MVC.Helpers.Global;
using DK.TwoFactorAuth;
using secret = DK.TwoFactorAuth.Secret;

namespace DFM.MVC.Models
{
	public class UsersConfigModel : BaseLoggedModel
	{
		public UsersConfigModel()
		{
			Main = new MainConfig(Admin, Current.User.Config);
			Info = new UserInfo(Safe);
			TFA = new TFAForm(Safe, Current);
			Theme = new ThemeOptions(Admin, Current.User.Config);
		}

		public MainConfig Main { get; set; }
		public UserInfo Info { get; set; }
		public TFAForm TFA { get; set; }
		public ThemeOptions Theme { get; set; }

		public Form ActiveForm { get; set; }

		public enum Form
		{
			Options,
			Password,
			Email,
			Theme,
			TFA,
		}


		public class MainConfig : IMainConfig
		{
			public MainConfig(AdminService admin, Config config)
			{
				this.admin = admin;

				var languageDictionary =
					PlainText.AcceptedLanguage()
						.ToDictionary(l => l, l => MultiLanguage.Dictionary["Language" + l]);

				LanguageList = SelectListExtension.CreateSelect(languageDictionary);
				TimeZoneList = SelectListExtension.CreateSelect(DateTimeGMT.TimeZoneList());

				UseCategories = config.UseCategories;
				SendMoveEmail = config.SendMoveEmail;
				MoveCheck = config.MoveCheck;
				Wizard = config.Wizard;

				Language = config.Language;
				TimeZone = config.TimeZone;
			}


			private readonly AdminService admin;

			public Boolean? UseCategories { get; set; }
			public Boolean? SendMoveEmail { get; set; }
			public Boolean? MoveCheck { get; set; }
			public Boolean? Wizard { get; set; }

			public Boolean UseCategoriesCheck
			{
				get => UseCategories.HasValue && UseCategories.Value;
				set => UseCategories = value;
			}

			public Boolean SendMoveEmailCheck
			{
				get => SendMoveEmail.HasValue && SendMoveEmail.Value;
				set => SendMoveEmail = value;
			}

			public Boolean MoveCheckCheck
			{
				get => MoveCheck.HasValue && MoveCheck.Value;
				set => MoveCheck = value;
			}

			public Boolean WizardCheck
			{
				get => Wizard.HasValue && Wizard.Value;
				set => Wizard = value;
			}

			public String Language { get; set; }
			public String TimeZone { get; set; }

			public SelectList TimeZoneList { get; set; }
			public SelectList LanguageList { get; set; }


			internal IList<String> Save()
			{
				var errors = new List<String>();

				try
				{
					admin.UpdateConfig(this);
					ErrorAlert.Add("ConfigChanged");
				}
				catch (DFMCoreException e)
				{
					errors.Add(MultiLanguage.Dictionary[e]);
				}

				return errors;
			}
		}

		public class UserInfo : IPasswordForm
		{
			public UserInfo(SafeService safe)
			{
				this.safe = safe;
			}

			private readonly SafeService safe;

			public String Email { get; set; }

			public String CurrentPassword { get; set; }

			public String Password { get; set; }
			public String RetypePassword { get; set; }

			public IList<String> ChangePassword()
			{
				var errors = new List<String>();

				try
				{
					safe.ChangePassword(CurrentPassword, this);
					ErrorAlert.Add("PasswordChanged");
				}
				catch (DFMCoreException e)
				{
					errors.Add(MultiLanguage.Dictionary[e]);
				}

				return errors;
			}

			public IList<String> UpdateEmail()
			{
				var errors = new List<String>();

				try
				{
					safe.UpdateEmail(CurrentPassword, Email);

					ErrorAlert.Add("EmailUpdated");
				}
				catch (DFMCoreException e)
				{
					errors.Add(MultiLanguage.Dictionary[e]);
				}

				return errors;
			}
		}

		public class TFAForm
		{
			public TFAForm(SafeService safe, Current current)
			{
				this.safe = safe;
				this.current = current;
				Secret = secret.Generate();
			}

			private readonly SafeService safe;
			private readonly Current current;

			public String CurrentPassword { get; set; }

			public String Secret { get; set; }
			public String Code { get; set; }

			private String identifier => current.User.Email;
			private String key => Base32.Convert(Secret);
			public String Url => $"otpauth://totp/DfM:{identifier}?secret={key}";

			public IList<String> Activate()
			{
				var errors = new List<String>();

				try
				{
					safe.UpdateTFA(Secret, Code, CurrentPassword);
					ErrorAlert.Add("TFAAuthenticated");
				}
				catch (DFMCoreException e)
				{
					errors.Add(MultiLanguage.Dictionary[e]);
				}

				return errors;
			}
		}

		public class ThemeOptions
		{
			public ThemeOptions(AdminService admin, Config config)
			{
				this.admin = admin;
				Theme = config.Theme;

				ThemeList =
					Enum.GetValues(typeof (BootstrapTheme))
						.Cast<BootstrapTheme>()
						.Where(bt => bt != 0)
						.OrderBy(bt => bt.ToString())
						.ToList();
			}

			private readonly AdminService admin;

			public BootstrapTheme Theme { get; set; }

			public IList<BootstrapTheme> ThemeList { get; private set; }

			public IList<String> Change()
			{
				var errors = new List<String>();

				try
				{
					admin.ChangeTheme(Theme);
					ErrorAlert.Add("ConfigChanged");
				}
				catch (DFMCoreException e)
				{
					errors.Add(MultiLanguage.Dictionary[e]);
				}

				return errors;
			}
		}

	}
}



