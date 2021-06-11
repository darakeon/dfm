using System;
using DFM.MVC.Models.Configs;

namespace DFM.MVC.Models
{
	public class ConfigsConfigModel : BaseSiteModel
	{
		public ConfigsConfigModel()
		{
			Main = new MainConfig(admin, current, translator, errorAlert);
			Info = new UserInfo(safe, translator, errorAlert);
			TFA = new TFAForm(safe, current, translator, errorAlert);
			ThemeOpt = new ThemeOptions(admin, Theme, translator, errorAlert);
		}

		public MainConfig Main { get; set; }
		public UserInfo Info { get; set; }
		public TFAForm TFA { get; set; }
		public ThemeOptions ThemeOpt { get; set; }

		public Form ActiveForm { get; set; }

		public String LanguageFieldName =>
			getName<ConfigsConfigModel>(m => m.Main.Language);

		public String BackTo { get; set; }

		public String BackFieldName =>
			getName<ConfigsConfigModel>(m => m.BackTo);

		// ReSharper disable once UnusedMember.Global
		public enum Form
		{
			Main,
			Password,
			Email,
			Theme,
			TFA,
		}
	}
}
