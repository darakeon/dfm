using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;

namespace DFM.MVC.Models
{
	public class SettingsThemeModel : BaseSiteModel, SettingsModel
	{
		public SettingsThemeModel()
		{
			NewTheme = Theme;
			BrightnessList = EnumX.AllExcept(ThemeBrightness.None);
			ColorList = EnumX.AllExcept(ThemeColor.None);
		}

		public String BackTo { get; set; }

		public Theme NewTheme { get; set; }

		public IList<ThemeBrightness> BrightnessList { get; }
		public IList<ThemeColor> ColorList { get; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				clip.ChangeTheme(NewTheme);
				errorAlert.Add("SettingsChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
