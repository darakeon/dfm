using System;
using DFM.BusinessLogic.Helpers;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Api.Models
{
	public class Environment
	{
		public Environment(BootstrapTheme theme, String language)
		{
			MobileTheme = theme.Simplify().ToString();
			Language = language;
		}

		public String MobileTheme { get; }
		public String Language { get; }
	}
}