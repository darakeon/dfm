using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Generic.Datetime;
using DFM.Language;
using Keon.MVC.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Models
{
	public class SettingsIndexModel : BaseSiteModel, SettingsModel
	{
		public SettingsIndexModel()
		{
			var languageDictionary =
				PlainText.AcceptedLanguages()
					.ToDictionary(l => l, l => translator["Language" + l]);

			LanguageList = SelectListExtension.CreateSelect(languageDictionary);
			TimeZoneList = SelectListExtension.CreateSelect(TZ.All);

			Info = new SettingsInfo
			{
				UseCategories = current.UseCategories,
				UseAccountsSigns = current.UseAccountsSigns,
				SendMoveEmail = current.SendMoveEmail,
				MoveCheck = current.MoveCheck,
				UseCurrency = current.UseCurrency,
				Wizard = current.Wizard,
				Language = current.Language,
				TimeZone = current.TimeZone,
			};
		}

		public readonly SettingsInfo Info;

		public SelectList TimeZoneList { get; set; }

		public String NewTimeZone
		{
			get => Info.TimeZone;
			set => Info.TimeZone = value;
		}

		public SelectList LanguageList { get; set; }

		public String NewLanguage
		{
			get => Info.Language;
			set => Info.Language = value;
		}

		public Boolean UseCategoriesCheck
		{
			get => Info.UseCategories.HasValue && Info.UseCategories.Value;
			set => Info.UseCategories = value;
		}

		public Boolean UseAccountsSignsCheck
		{
			get => Info.UseAccountsSigns.HasValue && Info.UseAccountsSigns.Value;
			set => Info.UseAccountsSigns = value;
		}

		public Boolean SendMoveEmailCheck
		{
			get => Info.SendMoveEmail.HasValue && Info.SendMoveEmail.Value;
			set => Info.SendMoveEmail = value;
		}

		public Boolean MoveCheckCheck
		{
			get => Info.MoveCheck.HasValue && Info.MoveCheck.Value;
			set => Info.MoveCheck = value;
		}

		public Boolean UseCurrency
		{
			get => Info.UseCurrency.HasValue && Info.UseCurrency.Value;
			set => Info.UseCurrency = value;
		}

		public Boolean WizardCheck
		{
			get => Info.Wizard.HasValue && Info.Wizard.Value;
			set => Info.Wizard = value;
		}

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				clip.UpdateSettings(Info);
				errorAlert.Add("SettingsChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public String LanguageFieldName =>
			getName<SettingsIndexModel>(m => m.NewLanguage);

		public String BackTo { get; set; }

		public String BackFieldName => nameof(BackTo);
	}
}
