using DFM.Entities.Enums;
using DFM.Multilanguage.Emails;

namespace DFM.BusinessLogic.Helpers
{
	public static class ThemeExtension
	{
		public static SimpleTheme Simplify(this BootstrapTheme theme)
		{
			return
				theme > 0 ? SimpleTheme.Dark :
				theme < 0 ? SimpleTheme.Light :
							SimpleTheme.None;
		}
	}
}
