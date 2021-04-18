using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Generic;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models.UserConfig
{
	public class ThemeOptions
	{
		public ThemeOptions(AdminService admin, Theme theme, Translator translator, ErrorAlert errorAlert)
		{
			this.admin = admin;
			this.translator = translator;
			this.errorAlert = errorAlert;
			Theme = theme;

			BrightnessList = EnumX.AllExcept(ThemeBrightness.None);
			ColorList = EnumX.AllExcept(ThemeColor.None);
		}

		private readonly AdminService admin;
		private readonly Translator translator;
		private readonly ErrorAlert errorAlert;

		public Theme Theme { get; set; }

		public IList<ThemeBrightness> BrightnessList { get; }
		public IList<ThemeColor> ColorList { get; }

		public IList<String> Change()
		{
			var errors = new List<String>();

			try
			{
				admin.ChangeTheme(Theme);
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
