using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Services;
using DFM.Generic.Datetime;
using DFM.Language;
using DFM.MVC.Helpers.Global;
using Keon.MVC.Forms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Models.UserConfig
{
	public class MainConfig : ConfigInfo
	{
		public MainConfig(AdminService admin, Current current, Translator translator, ErrorAlert errorAlert)
		{
			this.admin = admin;
			this.translator = translator;
			this.errorAlert = errorAlert;

			var languageDictionary =
				PlainText.AcceptedLanguage()
					.ToDictionary(l => l, l => translator["Language" + l]);

			LanguageList = SelectListExtension.CreateSelect(languageDictionary);
			TimeZoneList = SelectListExtension.CreateSelect(TZ.All);

			UseCategories = current.UseCategories;
			SendMoveEmail = current.SendMoveEmail;
			MoveCheck = current.MoveCheck;
			Wizard = current.Wizard;

			Language = current.Language;
			TimeZone = current.TimeZone;
		}


		private readonly AdminService admin;
		private readonly Translator translator;
		private readonly ErrorAlert errorAlert;

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

		public SelectList TimeZoneList { get; set; }
		public SelectList LanguageList { get; set; }


		internal IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				admin.UpdateConfig(this);
				errorAlert.Add("ConfigChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
