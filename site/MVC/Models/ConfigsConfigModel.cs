using System;
using System.Collections.Generic;
using DFM.MVC.Models.Configs;

namespace DFM.MVC.Models
{
	public class ConfigsConfigModel : BaseSiteModel
	{
		public ConfigsConfigModel()
		{
			Info = new UserInfo(safe, translator, errorAlert);
			TFA = new TFAForm(safe, current, translator, errorAlert);
			ThemeOpt = new ThemeOptions(admin, Theme, translator, errorAlert);
		}

		public UserInfo Info { get; set; }
		public TFAForm TFA { get; set; }
		public ThemeOptions ThemeOpt { get; set; }

		public Form ActiveForm { get; set; }

		public String BackTo { get; set; }

		// ReSharper disable once UnusedMember.Global
		public enum Form
		{
			Password,
			Email,
			Theme,
			TFA,
		}
	}
}
