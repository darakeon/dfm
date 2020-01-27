using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models.UserConfig
{
	public class ThemeOptions
	{
		public ThemeOptions(AdminService admin, BootstrapTheme theme, Translator translator, ErrorAlert errorAlert)
		{
			this.admin = admin;
			this.translator = translator;
			this.errorAlert = errorAlert;
			Theme = theme;

			ThemeList =
				Enum.GetValues(typeof (BootstrapTheme))
					.Cast<BootstrapTheme>()
					.Where(bt => bt != 0)
					.OrderBy(bt => bt.ToString())
					.ToList();
		}

		private readonly AdminService admin;
		private readonly Translator translator;
		private readonly ErrorAlert errorAlert;

		public BootstrapTheme Theme { get; set; }

		public IList<BootstrapTheme> ThemeList { get; }

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
