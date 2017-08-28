using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DK.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Multilanguage;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersConfigModel : BaseLoggedModel
	{
		public UsersConfigModel()
		{
			Main = new MainConfig(Admin, Current.User.Config);
			Info = new UserInfo(Safe);
			Theme = new ThemeOptions(Admin, Current.User.Config);
		}

		public MainConfig Main { get; set; }
		public UserInfo Info { get; set; }
		public ThemeOptions Theme { get; set; }
		
		public Form ActiveForm { get; set; }

		public enum Form
		{
			Options,
			Password,
			Email,
			Theme
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
				Language = config.Language;
				TimeZone = config.TimeZone;
			}


			private readonly AdminService admin;

			public Boolean? UseCategories { get; set; }
			public Boolean? SendMoveEmail { get; set; }
			public Boolean? MoveCheck { get; set; }

			public Boolean UseCategoriesCheck
			{
				get { return UseCategories.HasValue && UseCategories.Value; }
				set { UseCategories = value; }
			}

			public Boolean SendMoveEmailCheck
			{
				get { return SendMoveEmail.HasValue && SendMoveEmail.Value; }
				set { SendMoveEmail = value; }
			}

			public Boolean MoveCheckCheck
			{
				get { return MoveCheck.HasValue && MoveCheck.Value; }
				set { MoveCheck = value; }
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
					var pathAction = Url.Action("UserVerification", "Tokens");
					var pathDisable = Url.Action("Disable", "Tokens");
					
					safe.UpdateEmail(CurrentPassword, Email, pathAction, pathDisable);
					
					ErrorAlert.Add("EmailUpdated");
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



