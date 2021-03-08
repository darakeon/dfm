using System;
using DFM.Entities.Enums;
using emailTheme = DFM.Language.Emails.Theme;

namespace DFM.BusinessLogic.Helpers
{
	public static class ThemeExtension
	{
		public static emailTheme Convert(this Theme theme)
		{
			return (emailTheme)(Int32)theme.Brightness();
		}
	}
}
