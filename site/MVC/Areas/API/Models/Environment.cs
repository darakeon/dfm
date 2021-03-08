using System;
using DFM.BusinessLogic.Helpers;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Api.Models
{
	public class Environment
	{
		public Environment(Theme theme, String language)
		{
			MobileTheme = theme.Convert().ToString();
			Language = language;
		}

		public String MobileTheme { get; }
		public String Language { get; }
	}
}
