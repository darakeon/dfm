﻿using System;
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
	public class ConfigsIndexModel : BaseSiteModel, ConfigsModel
	{
		public ConfigsIndexModel()
		{
			var languageDictionary =
				PlainText.AcceptedLanguages()
					.ToDictionary(l => l, l => translator["Language" + l]);

			LanguageList = SelectListExtension.CreateSelect(languageDictionary);
			TimeZoneList = SelectListExtension.CreateSelect(TZ.All);

			Info = new ConfigInfo
			{
				UseCategories = current.UseCategories,
				SendMoveEmail = current.SendMoveEmail,
				MoveCheck = current.MoveCheck,
				Wizard = current.Wizard,
				Language = current.Language,
				TimeZone = current.TimeZone,
			};
		}

		public readonly ConfigInfo Info;

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
				admin.UpdateConfig(Info);
				errorAlert.Add("ConfigChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public String LanguageFieldName =>
			getName<ConfigsIndexModel>(m => m.NewLanguage);

		public String BackTo { get; set; }

		public String BackFieldName =>
			getName<ConfigsIndexModel>(m => m.BackTo);
	}
}
